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
    public class FluentEvent : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdEvent);
            modelBuilder.Property(x => x.NameEvent).IsRequired();
            modelBuilder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId);
        }
    }
}
