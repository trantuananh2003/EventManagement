using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.Repository
{
    public class PurchasedTicketRepository : Repository<PurchasedTicket>, IPurchasedTicketRepository
    {
        private readonly ApplicationDbContext _db;
        public PurchasedTicketRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task Update(PurchasedTicket entity)
        {
            var ticket = await _db.PurchasedTickets.FirstOrDefaultAsync(t => t.IdPurchasedTicket == entity.IdPurchasedTicket);
            if (ticket != null)
            {
                ticket.FullName = entity.FullName;
                ticket.Email = entity.Email;
                ticket.Phone = entity.Phone;
                _db.Update(ticket);
            }
        }
    }
}
