using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.FluentConfig
{
    public class FluentRole : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> modelBuilder)
        {
            //remove the current idenx
            modelBuilder.HasIndex(x => x.NormalizedName).IsUnique(false);
            // add composite constraint 
            modelBuilder.HasIndex(x => new { x.NormalizedName, x.OrganizationId }).IsUnique();
        }
    }
}
