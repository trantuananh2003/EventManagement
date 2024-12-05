using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Queries.ModelDto
{
    public class TicketStatisticDto
    {
        public string TicketName { get; set; }
        public int SoldQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

}
