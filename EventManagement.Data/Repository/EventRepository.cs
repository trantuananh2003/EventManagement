using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _db;

        public EventRepository(ApplicationDbContext db) : base(db) {
            _db = db;
        }

        public async Task<Event> UpdateAsync(Event entity)
        {
            var model = _db.Events.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
