using EventManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.IRepository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket> UpdateAsync(Ticket entity);
    }
}
