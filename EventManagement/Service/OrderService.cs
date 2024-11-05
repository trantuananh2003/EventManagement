using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;

namespace EventManagement.Service
{
    public interface IOrderService
    {

    }

    public class OrderService : IOrderService
    {
        private readonly IOrderHeaderRepository _dbOrderHeader;
        private readonly IOrderDetailRepository _dbOrderDetail;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IOrderHeaderRepository dbOrderHeader, IOrderDetailRepository dbOrderDetail) {
            _mapper = mapper;
            _dbOrderHeader = dbOrderHeader;
            _dbOrderDetail = dbOrderDetail;
        }

        public void CreateOrder(OrderHeaderCreateDto model)
        {
            var orderHeader = _mapper.Map<OrderHeader>(model);
            //_dbOrderHeader.Add(orderHeader);
            //_dbOrderHeader.Save();
            foreach (var item in model.OrderDetails)
            {
                var orderDetail = _mapper.Map<OrderDetail>(item);
                orderDetail.OrderHeaderId = orderHeader.IdOrderHeader;
                //_dbOrderDetail.Add(orderDetail);
            }
            //_dbOrderDetail.Save();
        }

    }
}
