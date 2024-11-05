using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.FluentConfig
{
    public class FluentEventDate : IEntityTypeConfiguration<EventDate>
    {
        public void Configure(EntityTypeBuilder<EventDate> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdEventDate);
            modelBuilder.Property(x => x.DateTitle).IsRequired();
            modelBuilder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId);
            modelBuilder.Property(x => x.ScheduledDate).HasColumnType("date").IsRequired();
            modelBuilder.Property(x => x.StartTime).HasColumnType("time").IsRequired();
            modelBuilder.Property(x => x.EndTime).HasColumnType("time").IsRequired();
        }
    }
}
