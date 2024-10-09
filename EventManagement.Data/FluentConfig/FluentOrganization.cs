using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Data.FluentConfig
{
    public class FluentOrganization: IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> modelBuilder)
        {
            modelBuilder.HasKey(x => x.IdOrganization);
            modelBuilder.HasOne(x => x.User).WithOne().HasForeignKey<Organization>(x => x.IdUserOwner);
        }
    }
}
