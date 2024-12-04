using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class EventTag
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
