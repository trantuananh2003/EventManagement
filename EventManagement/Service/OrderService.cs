using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EventManagement.Service
{
    public enum EOrderCreate
    {
        OutOfStock,
        NotFoundItem,
        Done,
    }

    public interface IOrderService
    {
        Task<(List<OverviewOrderDto>, int)> GetAllOrderByIdUser(string userId, string searchString, int pageSize, int pageNumber);

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
            var listOrderQuery = _dbOrderHeader.GetListOverviewOrderDtos(userId);
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
                    var orderHeader = _mapper.Map<OrderHeader>(model);
                    orderHeader.IdOrderHeader = Guid.NewGuid().ToString();
                    orderHeader.OrderDetails = null;
                    orderHeader.Status = "Success";
                    orderHeader.OrderDate = DateTime.Now;
                    orderHeader.TotalItem = 0;
                    await _dbOrderHeader.CreateAsync(orderHeader);

                    foreach (var orderDetailDto in model.OrderDetails)
                    {
                        var ticketEntity = await _dbTicket.GetAsync(u => u.IdTicket == orderDetailDto.TicketId, tracked: true);
                        if (ticketEntity == null)
                        {
                            transaction.Rollback();
                            return EOrderCreate.NotFoundItem;
                        }

                        var remainingQuantity = ticketEntity.Quantity - orderDetailDto.Quantity;
                        ticketEntity.Quantity = remainingQuantity;
                        if (remainingQuantity < 0)
                        {
                            transaction.Rollback();
                            return EOrderCreate.OutOfStock;
                        }

                        var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
                        orderDetail.OrderHeaderId = orderHeader.IdOrderHeader;
                        orderDetail.IdOrderDetail = Guid.NewGuid().ToString();
                        orderDetail.Price = ticketEntity.Price;
                        orderDetail.NameTicket = ticketEntity.NameTicket;

                        orderHeader.TotalItem++;
                        orderHeader.PriceTotal += orderDetail.Price;
                        orderHeader.EventId = ticketEntity.EventId;

                        await _dbOrderDetail.CreateAsync(orderDetail);
                        await _purchasedTicketService.CreatePurchased(orderDetail.IdOrderDetail, orderHeader.IdOrderHeader);
                    }

                    await _dbOrderHeader.SaveAsync();
                    transaction.Commit();

                    return EOrderCreate.Done;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
