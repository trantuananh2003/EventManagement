using EventManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IPurchasedTicketRepository: IRepository<PurchasedTicket>
    {
        Task Update(PurchasedTicket entity);
    }
}
