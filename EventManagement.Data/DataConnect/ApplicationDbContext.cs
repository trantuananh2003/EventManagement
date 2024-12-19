using EventManagement.Data.FluentConfig;
using EventManagement.Data.FluentConfig.FluentSupportChatRoom;
using EventManagement.Data.Models;
using EventManagement.Data.Models.ChatRoom;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.DataConnect
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<EventDate> EventDates { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<MemberOrganization> MemberOrganizations { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public DbSet<SupportChatRoom> SupportChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Tag> Tags { get; set; } //Chua dung toi
        public DbSet<EventTag> EventTags { get; set; } //Chua dung toi
        public DbSet<OverviewEvent> OverviewEvents { get; set; } //Chua dung toi
        public DbSet<RefreshToken> RefreshTokens { get; set; } // Chua dung toi

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new FluentOrganization());
            modelBuilder.ApplyConfiguration(new FluentEvent());
            modelBuilder.ApplyConfiguration(new FluentAgenda());
            modelBuilder.ApplyConfiguration(new FluentOverviewEvent());
            modelBuilder.ApplyConfiguration(new FluentEventDate());
            modelBuilder.ApplyConfiguration(new FluentTicket());
            modelBuilder.ApplyConfiguration(new FluentOrderHeader());
            modelBuilder.ApplyConfiguration(new FluentOrderDetail());
            modelBuilder.ApplyConfiguration(new FlunetMemberOrganization());
            modelBuilder.ApplyConfiguration(new FluentRole());
            modelBuilder.ApplyConfiguration(new FluentPurchasedTicket());
            modelBuilder.ApplyConfiguration(new FluentSupportChatRoom());
            modelBuilder.ApplyConfiguration(new FluentMessage());
            modelBuilder.ApplyConfiguration(new FluentTag());
            modelBuilder.ApplyConfiguration(new FluentEventTag());
        }
    }
}
