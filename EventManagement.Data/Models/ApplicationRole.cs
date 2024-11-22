using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string OrganizationId { get; set; }
        public string Description { get; set; }
    }
}
