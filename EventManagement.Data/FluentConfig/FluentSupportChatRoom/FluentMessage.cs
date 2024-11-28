using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EventManagement.Data.Models.ChatRoom;

namespace EventManagement.Data.FluentConfig.FluentSupportChatRoom
{
    public class FluentMessage : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> modelBuilder)
        {
            modelBuilder.HasKey(x => x.MessageId);
            modelBuilder.HasOne(x => x.SupportChatRoom).WithMany().HasForeignKey(x => x.SupportChatRoomId).IsRequired();
            modelBuilder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.SenderId).IsRequired();
            modelBuilder.Property(x => x.Content).IsRequired();
            modelBuilder.Property(x => x.IsSupport).IsRequired();
            modelBuilder.Property(x => x.SendAt)
                .HasColumnType("datetime")
                .HasDefaultValue(new DateTime(1753, 1, 1))
                .IsRequired();
        }
    }
}
