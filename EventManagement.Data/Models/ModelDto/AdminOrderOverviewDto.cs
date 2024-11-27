using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models.ModelDto
{
    public class AdminOrderOverviewDto
    {
        public string IdOrderHeader { get; set; }
        public string NameEvent { get; set; }
        public string NameBuyer { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
