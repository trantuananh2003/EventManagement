using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class MemberOrganization
    {
        public string MemberId { get; set; }
        public string IdUser { get; set; }
        public ApplicationUser User { get; set; }
        public string IdOrganization { get; set; }
        public Organization Organization { get; set; }
    }
}
