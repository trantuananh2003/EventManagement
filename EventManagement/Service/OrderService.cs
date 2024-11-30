using AutoMapper;
using EventManagement.Common;
using EventManagement.Data.Models;
using EventManagement.Data.Models.ModelDto;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrderDetailDtos;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EventManagement.Service
{
    public interface IOrderService
    {
        Task<(List<OverviewOrderDto>, int)> GetAllOrderByIdUser(string userId, string searchString, int pageSize, int pageNumber);
        Task<(List<AdminOrderOverviewDto>, int)> GetAllOrderByIdOrganization(string idOrganization, string searchString, int pageSize, int pageNumber);
        Task<OrderHeaderDto> GetOrderHeaderById(string idOrderHeader);
        Task<List<OrderDetailDto>> GetOrderDetailDto(string orderHeader);
        Task<EOrderCreate> CreateOrder(OrderHeaderCreateDto model);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderHeaderRepository _dbOrderHeader;
        private readonly IOrderDetailRepository _dbOrderDetail;
        private readonly ITicketRepository _dbTicket;
        private readonly IPurchasedTicketService _purchasedTicketService;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IOrderHeaderRepository dbOrderHeader, IOrderDetailRepository dbOrderDetail,
            ITicketRepository dbTicket, IPurchasedTicketService purchasedTicketService)
        {
            _mapper = mapper;
            _dbOrderHeader = dbOrderHeader;
            _dbOrderDetail = dbOrderDetail;
            _dbTicket = dbTicket;
            _purchasedTicketService = purchasedTicketService;
        }

        public async Task<(List<OverviewOrderDto>, int)> GetAllOrderByIdUser(string userId, string searchString, int pageSize, int pageNumber)
        {
            var listOrderQuery = _dbOrderHeader.GetUserOrders(userId);
            if (!string.IsNullOrEmpty(searchString))
            {
                listOrderQuery = listOrderQuery.Where(x => x.NameEvent.ToLower().Contains(searchString.ToLower()));
            }
            var totalRow = await listOrderQuery.CountAsync();
            if (pageSize > 0)
            {
                listOrderQuery = listOrderQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
            var listOrderFromData = await listOrderQuery.ToListAsync();

            var orderDto = _mapper.Map<List<OverviewOrderDto>>(listOrderFromData);
            return (orderDto, totalRow);
        }
        public async Task<EOrderCreate> CreateOrder(OrderHeaderCreateDto model)
        {
            using (var transaction = _dbOrderHeader.BeginTransaction())
            {
                try
                {
                    // Map dữ liệu từ DTO sang entity
                    var orderHeader = _mapper.Map<OrderHeader>(model);
                    orderHeader.IdOrderHeader = Guid.NewGuid().ToString();
                    orderHeader.OrderDetails = null;
                    orderHeader.Status = "Success";
                    orderHeader.OrderDate = DateTime.Now;
                    orderHeader.TotalItem = 0;

                    await _dbOrderHeader.CreateAsync(orderHeader);

                    // Xử lý từng chi tiết đơn hàng
                    foreach (var orderDetailDto in model.OrderDetails)
                    {
                        var ticketEntity = await _dbTicket.GetAsync(
                            u => u.IdTicket == orderDetailDto.TicketId,
                            tracked: true
                        );

                        // Kiểm tra vé có tồn tại không
                        if (ticketEntity == null)
                        {
                            transaction.Rollback();
                            return EOrderCreate.NotFoundItem;
                        }

                        // Cập nhật số lượng vé
                        var remainingQuantity = ticketEntity.Quantity - orderDetailDto.Quantity;
                        ticketEntity.Quantity = remainingQuantity;

                        // Kiểm tra hết hàng
                        if (remainingQuantity < 0 
                            || ticketEntity.Status == EStatusTicket.SoldOut.ToString() 
                            || ticketEntity.Visibility == EVisibilityTicket.Hidden.ToString()
                            || ticketEntity.SaleMethod == ESaleMethodTicket.OnSite.ToString())
                        {
                            transaction.Rollback();
                            return EOrderCreate.OutOfStock;
                        }

                        // Tạo chi tiết đơn hàng
                        var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
                        orderDetail.OrderHeaderId = orderHeader.IdOrderHeader;
                        orderDetail.IdOrderDetail = Guid.NewGuid().ToString();
                        orderDetail.Price = ticketEntity.Price;
                        orderDetail.NameTicket = ticketEntity.NameTicket;

                        // Cập nhật thông tin đơn hàng
                        orderHeader.TotalItem++;
                        orderHeader.PriceTotal += orderDetail.Price;
                        orderHeader.EventId = ticketEntity.EventId;

                        await _dbOrderDetail.CreateAsync(orderDetail);

                        // Tạo vé đã mua
                        for (int i = 0; i < orderDetail.Quantity; i++)
                        {
                            await _purchasedTicketService.CreatePurchased(
                                orderDetail.IdOrderDetail,
                                orderHeader.IdOrderHeader
                            );
                        }
                    }

                    // Lưu thay đổi và commit transaction
                    await _dbOrderHeader.SaveAsync();
                    transaction.Commit();

                    return EOrderCreate.Done;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw; // Ghi log nếu cần thiết
                }
            }
        }

        public async Task<(List<AdminOrderOverviewDto>, int)> GetAllOrderByIdOrganization(string idOrganization, string searchString,
               int pageSize=0, int pageNumber=1)
        {
            var (listOrder, total) = await _dbOrderHeader.GetAdminOrders(idOrganization, searchString, pageSize, pageNumber);

            var orderDto = _mapper.Map<List<AdminOrderOverviewDto>>(listOrder);
            return (orderDto, total);
        }

        public async Task<List<OrderDetailDto>> GetOrderDetailDto(string idOrderHeader)
        {             
            var orderDetails = await _dbOrderDetail.GetAllAsync(x => x.OrderHeaderId == idOrderHeader, includeProperties: "PurchasedTickets");
            var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
            return orderDetailsDto;
        }

        public async Task<OrderHeaderDto> GetOrderHeaderById(string idOrderHeader)
        {
            var orderHeaderEntity = await _dbOrderHeader.GetAsync(x => x.IdOrderHeader == idOrderHeader, includeProperties: "User");
            var orderHeaderDto = _mapper.Map<OrderHeaderDto>(orderHeaderEntity);
            return orderHeaderDto;
        }
    }
}
