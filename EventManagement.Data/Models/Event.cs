using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class Event
    {
        public string IdEvent { get; set; }
        public string NameEvent { get; set; }
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Location { get; set; }
        public string Privacy { get; set; } //Public || Private
        public string Status { get; set; } //OnSale || SoldOut || Cancelled || Postponted
        public string EventType { get; set; } //Multiple || One
    }
}
