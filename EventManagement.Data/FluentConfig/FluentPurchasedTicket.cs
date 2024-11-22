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
    public class FluentPurchasedTicket : IEntityTypeConfiguration<PurchasedTicket>
    {
        public void Configure(EntityTypeBuilder<PurchasedTicket> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdPurchasedTicket);
            modelBuilder.HasOne(x => x.OrderDetail).WithMany().HasForeignKey(x => x.OrderDetailId).IsRequired();
            modelBuilder.HasOne(x => x.OrderHeader).WithMany().HasForeignKey(x => x.OrderHeaderId).IsRequired();
            modelBuilder.Property(x => x.Status).IsRequired();
        }
    }
}
