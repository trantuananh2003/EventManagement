using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.FluentConfig
{
    public class FluentOrderHeader: IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdOrderHeader);
            modelBuilder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            modelBuilder.HasOne(x => x.Event).WithMany(e => e.OrderHeaders).HasForeignKey(x => x.EventId); 
            modelBuilder.Property(x => x.NumberPhone).IsRequired();
            modelBuilder.Property(x => x.PriceTotal).IsRequired();
            modelBuilder.Property(x => x.OrderDate).IsRequired();
            modelBuilder.Property(x => x.Status).IsRequired();
        }
    }
}
