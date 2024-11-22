using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class PurchasedTicket
    {
        public string IdPurchasedTicket { get; set; }
        public string OrderDetailId { get; set; }
        public OrderDetail OrderDetail { get; set; }

        public string OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
    }
}
