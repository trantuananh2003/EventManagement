using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class Ticket
    {
        public string IdTicket { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string EventDateId { get; set; }
        public EventDate EventDate { get; set; }
        public string NameTicket { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public string Description { get; set; }
        // Enum for Status
        public string Status { get; set; }

        // Enum for Visibility
        public string Visibility { get; set; }

        // Enum for Sale Method
        public string SaleMethod { get; set; }
    }
}
