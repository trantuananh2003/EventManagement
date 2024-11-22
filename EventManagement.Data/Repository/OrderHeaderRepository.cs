
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Queries.ModelDto;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader entity)
        {
            _db.OrderHeaders.Update(entity);
        }

        public IQueryable<DataOverviewOrderDto> GetListOverviewOrderDtos(string IdUser)
        {
            var modelQuery = _db.OrderHeaders
                .Where(oh => oh.UserId == IdUser) // Lọc theo UserId
                .Join(
                    _db.Events,
                    oh => oh.EventId, // Khóa từ OrderHeaders
                    e => e.IdEvent,   // Khóa từ Events
                    (oh, e) => new
                    {
                        oh.IdOrderHeader,
                        oh.PriceTotal,
                        oh.OrderDate,
                        oh.Status,
                        e.NameEvent,
                        e.UrlImage
                    }
                )
                .Join(_db.OrderDetails, u => u.IdOrderHeader, od => od.OrderHeaderId, (dto, od) => new
                {
                    dto.IdOrderHeader,
                    dto.NameEvent,
                    dto.UrlImage,
                    dto.OrderDate,
                    dto.Status,
                    dto.PriceTotal,
                    od.Quantity,
                })
                .GroupBy(x => new { x.IdOrderHeader, x.NameEvent, x.UrlImage, x.PriceTotal })
                .Select(group => new DataOverviewOrderDto
                {
                    IdOrderHeader = group.Key.IdOrderHeader,
                    NameEvent = group.Key.NameEvent,
                    UrlImage = group.Key.UrlImage,
                    TotalPrice = group.Key.PriceTotal,
                    OrderDate = group.Select(x => x.OrderDate).FirstOrDefault(),
                    Status = group.Select(x => x.Status).FirstOrDefault(),
                    TotalTicket = group.Sum(x => x.Quantity)
                });
            return modelQuery;
        }

    }
}
