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

        public void Update(EventDate entity)
        {
            _db.Update(entity);
        }

    }
}
