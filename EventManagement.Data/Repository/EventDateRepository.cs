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
    public class EventDateRepository : Repository<EventDate>, IEventDateRepository
    {
        public ApplicationDbContext _db;

        public EventDateRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<EventDate> UpdateAsync(EventDate entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllList(List<EventDate> listItem,string id)
        {
            var list = _db.EventDates.Where(x => x.EventId == id).ToList();
            _db.EventDates.RemoveRange(list);
            _db.EventDates.AddRange(listItem);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
