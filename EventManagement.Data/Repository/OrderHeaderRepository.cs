
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Models.ModelDto;
using EventManagement.Data.Queries.ModelDto;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks.Dataflow;

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

        public IQueryable<UserOrderOverviewDto> GetUserOrders(string IdUser)
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
                .Select(group => new UserOrderOverviewDto
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

        public async Task<(IEnumerable<AdminOrderOverviewDto>, int)> GetAdminOrders(string IdOrganization, string searchString,
            int pageSize = 0, int pageNumber = 1)
        {
            var modelQuery = from e in _db.Events
                             where e.OrganizationId == IdOrganization
                             join oh in _db.OrderHeaders on e.IdEvent equals oh.EventId
                             select new AdminOrderOverviewDto
                             {
                                 IdOrderHeader = oh.IdOrderHeader,
                                 NameEvent = e.NameEvent,
                                 NameBuyer = oh.User.UserName,
                                 InvoiceDate = oh.OrderDate,
                                 Status = oh.Status,
                                 TotalPrice = oh.PriceTotal
                             };
            IEnumerable<AdminOrderOverviewDto> result = null;

            if (pageSize > 0)
                result = await modelQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            else
                result = await modelQuery.ToListAsync();

            var total = await modelQuery.CountAsync();
            return (result, total);
        }
    }
}
