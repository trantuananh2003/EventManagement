using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class OrderDetail
    {
        public string IdOrderDetail { get; set; }

        public string OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public string TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public int Quantity { get; set; }
        public string NameTicket { get; set; }
        public int Price { get; set; }
    }
}
