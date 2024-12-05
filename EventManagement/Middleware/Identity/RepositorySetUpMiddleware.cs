using EventManagement.Data.Queries;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Data.Repository;

namespace EventManagement.Middleware.Identity
{
    public static class RepositorySetUpMiddleware
    {
        public static IServiceCollection AddServiceSetUpReposiotry(this IServiceCollection Services)
        {
            Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            Services.AddScoped<IEventRepository, EventRepository>();
            Services.AddScoped<IAgendaRepository, AgendaRepository>();
            Services.AddScoped<IEventDateRepository, EventDateRepository>();
            Services.AddScoped<ITicketRepository, TicketRepository>();
            Services.AddScoped<ISearchQuery, SearchQuery>();
            Services.AddScoped<IReportEvent, ReportEvent>();
            Services.AddScoped<IEventDetailViewQuery, EventDetailViewQuery>();
            Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            Services.AddScoped<IMemberOrganizationRepository, MemberOrganizationRepository>();
            Services.AddScoped<IPurchasedTicketRepository, PurchasedTicketRepository>();
            Services.AddScoped<ISupportChatRoomRepository, SupportChatRoomRepository>();
            Services.AddScoped<IMessageRepository, MessageRepository>();

            return Services;
        }
    }
}
