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
    public class FluentTicket : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdTicket);
            modelBuilder.Property(x => x.EventId).IsRequired();
            modelBuilder.Property(x => x.EventDateId).IsRequired();
            modelBuilder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId);
            modelBuilder.HasOne(x => x.EventDate).WithMany().HasForeignKey(x => x.EventDateId);
            modelBuilder.Property(x => x.NameTicket).IsRequired();
            modelBuilder.Property(X => X.SaleStartDate).HasColumnType("datetime").IsRequired();
            modelBuilder.Property(X => X.SaleEndDate).HasColumnType("datetime").IsRequired();
            modelBuilder.Property(x => x.Quantity).IsRequired();
            modelBuilder.Property(x => x.Price).IsRequired();
            modelBuilder.Property(x => x.Status).IsRequired();
            modelBuilder.Property(x => x.Visibility).IsRequired();
            modelBuilder.Property(x => x.SaleMethod).IsRequired();
        }
    }
}
