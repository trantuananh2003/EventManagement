using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Queries.ModelDto
{
    public class DataOverviewOrderDto
    {
        public string IdOrderHeader { get; set; }
        public string UrlImage { get; set; }
        public string NameEvent { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public int TotalTicket { get; set; }
        public int TotalPrice { get; set; }
    }
}
