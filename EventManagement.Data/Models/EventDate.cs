using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class EventDate
    {
        public string IdEventDate { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string DateTitle { get; set; }
        public DateTime ScheduledDate { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
