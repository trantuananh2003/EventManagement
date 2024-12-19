using EventManagement.Models;
using EventManagement.Service.OutService;

namespace EventManagement.Service.Configuration
{
    public static class ConfigurationServiceBS
    {
        public static void RegisterDIBussiness(this IServiceCollection Services)
        {
            Services.AddSingleton<IBlobService, BlobService>();
            Services.AddScoped<IOrganizationService, OrganizationService>();
            Services.AddScoped<IEventService, EventService>();
            Services.AddScoped<IAgendaService, AgendaService>();
            Services.AddScoped<IEventDateService, EventDateService>();
            Services.AddScoped<ITicketService, TicketService>();
            Services.AddScoped<ISearchService, SearchService>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPurchasedTicketService, PurchasedTicketService>();
            Services.AddScoped<ISupportChatService, SupportChatService>();
        }

        public static void RegisterMailService(this IServiceCollection services, IConfiguration configuration)
        {
            var mailSettings = configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);
            services.AddTransient<SendMailService>();
        }

    }
}
