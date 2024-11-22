using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser():base()
        { }

        public string FullName { get; set; }
    }
}
