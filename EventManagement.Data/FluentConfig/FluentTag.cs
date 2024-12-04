using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Data.FluentConfig
{
    public class FluentTag : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> modelBuilder)
        {
            modelBuilder.HasKey(x => x.TagId);
        }
    }
    public class FluentEventTag : IEntityTypeConfiguration<EventTag>
    {
        public void Configure(EntityTypeBuilder<EventTag> modelBuilder)
        {
            modelBuilder.HasKey(x => new { x.TagId, x.EventId });
            modelBuilder.HasOne(x => x.Tag).WithMany().HasForeignKey(x => x.TagId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
