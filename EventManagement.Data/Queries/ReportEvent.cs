using EventManagement.Data.DataConnect;
using EventManagement.Data.Queries.ModelDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Queries
{
    public interface IReportEvent
    {
        Task<List<TicketStatisticDto>> GetTicketStatisticsAsync(string eventId);
        Task<int> GetTotalOrderAsync(string eventId);
    }


    public class ReportEvent : IReportEvent
    {
        private readonly ApplicationDbContext _db;
        public ReportEvent (ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TicketStatisticDto>> GetTicketStatisticsAsync(string eventId)
        {
            var queryTicket =
            from t in _db.Tickets
            where t.EventId == eventId
            join od in _db.OrderDetails on t.IdTicket equals od.TicketId into orderDetails
            select new
            {
                Ticket = t,
                OrderDetails = orderDetails.DefaultIfEmpty() // Nếu không có OrderDetail, sẽ trả về null
            };

            var listTicket = await queryTicket.ToListAsync();

            var listTicketStatisticDto = new List<TicketStatisticDto>();
            foreach (var ticket in listTicket)
            {
                int totalRevenue = 0;
                int totalQuantity = 0;

                if(ticket.OrderDetails != null)
                {
                    totalRevenue = ticket.OrderDetails.Sum(od => od.Quantity * od.Price);
                    totalQuantity = ticket.OrderDetails.Sum(od => od.Quantity);
                }
             

                listTicketStatisticDto.Add(new TicketStatisticDto
                {
                    TicketName = ticket.Ticket.NameTicket,
                    RemainingQuantity = ticket.Ticket.Quantity,
                    TotalRevenue = totalRevenue,
                    SoldQuantity = totalQuantity,
                });
            }

            return listTicketStatisticDto;
        }
    
        public async Task<int> GetTotalOrderAsync(string eventId)
        {
            return await _db.OrderHeaders.Where(x => x.EventId == eventId).CountAsync();
        }
    }
}
