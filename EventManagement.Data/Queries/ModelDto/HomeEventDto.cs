using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Queries.ModelDto
{
    public class HomeEventDto
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string UrlImage { get; set; }
        public string Location { get; set; }
        public string NearDate { get; set; }
        public int PriceLow { get; set; }
        public int PriceHigh { get; set; }
    }
}
