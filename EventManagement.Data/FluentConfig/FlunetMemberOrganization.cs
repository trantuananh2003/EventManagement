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
    public class FlunetMemberOrganization : IEntityTypeConfiguration<MemberOrganization>
    {
        public void Configure(EntityTypeBuilder<MemberOrganization> modelBuilder)
        {
            modelBuilder.HasKey(x => x.MemberId);
            modelBuilder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.IdUser).IsRequired();
            modelBuilder.HasOne(x => x.Organization).WithMany().HasForeignKey(x=> x.IdOrganization).IsRequired();
        }
    }
}
