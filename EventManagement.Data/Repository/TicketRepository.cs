using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly ApplicationDbContext _db;

        public TicketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Ticket> UpdateAsync(Ticket entity)
        {
            _db.Tickets.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
