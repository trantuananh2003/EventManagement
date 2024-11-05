
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;

namespace EventManagement.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<OrderHeader> UpdateAsync(OrderHeader entity)
        {
            _db.OrderHeaders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
