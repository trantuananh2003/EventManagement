using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.FluentConfig
{
    public class FluentOverviewEvent : IEntityTypeConfiguration<OverviewEvent>
    {
        public void Configure(EntityTypeBuilder<OverviewEvent> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdOverviewEvent);
            modelBuilder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId);
            modelBuilder.Property(x => x.TypeOverView);
            modelBuilder.Property(x => x.Description);
        }
    }
}
