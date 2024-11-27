using EventManagement.Data.Models.ChatRoom;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.FluentConfig.FluentSupportChatRoom
{
    public class FluentSupportChatRoom : IEntityTypeConfiguration<SupportChatRoom>
    {
        public void Configure(EntityTypeBuilder<SupportChatRoom> modelBuilder)
        {
            modelBuilder.HasKey(x => x.SupportChatRoomId);
            modelBuilder.HasOne(x => x.Organization).WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
