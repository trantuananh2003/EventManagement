using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ITicketRepository TicketRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IPurchasedTicketRepository PurchasedTicketRepository { get; }
        IEventRepository EventRepository { get; }
        IEventDateRepository EventDateRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IMemberOrganizationRepository MemberOrganizationRepository { get; }

        Task SaveAsync();
    }
}
