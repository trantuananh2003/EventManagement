using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class OrderHeader
    {
        public string IdOrderHeader { get; set; } //Backend

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string EventId { get; set; }
        public Event Event { get; set; }

        public string NumberPhone { get; set; }
        public int PriceTotal { get; set; } //Backend
        public DateTime OrderDate { get; set; } //Backend
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItem { get; set; } //Backend
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
