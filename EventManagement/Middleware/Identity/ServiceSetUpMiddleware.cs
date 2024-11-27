using EventManagement.Security.Requirement;
using Microsoft.AspNetCore.Authorization;
using EventManagement.Service;
using EventManagement.Data.Models;

namespace EventManagement.Middleware.Identity
{
    public static class ServiceSetUpMiddleware
    {
        public static IServiceCollection AddServiceSetUp(this IServiceCollection Services)
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

            return Services;
        }
    }
}
