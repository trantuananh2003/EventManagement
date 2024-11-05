using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.FluentConfig
{
    public class FluentOrderDetail : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdOrderDetail);
            modelBuilder.HasOne(x => x.OrderHeader).WithMany(u => u.OrderDetails).HasForeignKey(x=>x.OrderHeaderId);
            modelBuilder.HasOne(x => x.Ticket).WithOne().HasForeignKey<OrderDetail>(x => x.TicketId);
            modelBuilder.Property(x => x.Quantity).IsRequired();
            modelBuilder.Property(x => x.Price).IsRequired();
            modelBuilder.Property(x => x.NameTicket).IsRequired();
        }
    }
}
