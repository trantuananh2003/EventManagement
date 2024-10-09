using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.FluentConfig
{
    public class FluentAgenda : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdAgenda);
            modelBuilder.Property(x => x.Title).IsRequired();
            modelBuilder.HasOne(x => x.Event).WithMany().HasForeignKey(x => x.EventId);
            modelBuilder.Property(x => x.StartTime).IsRequired();
            modelBuilder.Property(x => x.EndTime).IsRequired();
        }
    }
}
