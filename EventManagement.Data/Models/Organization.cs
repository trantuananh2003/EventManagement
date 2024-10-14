using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class Organization 
    {

        public string IdOrganization { get; set; }
        public string IdUserOwner { get; set; }
        public ApplicationUser User { get; set; }
        public string NameOrganization { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
