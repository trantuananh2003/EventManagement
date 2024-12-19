using EventManagement.Data.DataConnect;
using EventManagement.Data.Queries;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventManagement.Data.Dapper;

namespace EventManagement.Data.Configuration
{
    public static class ConfigurationServiceDb
    {
        public static void RegisterContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultSQLConnection"));
            });
        }

        public static void RegisterDIRepository(this IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            Services.AddScoped<IEventRepository, EventRepository>();
            Services.AddScoped<IAgendaRepository, AgendaRepository>();
            Services.AddScoped<IEventDateRepository, EventDateRepository>();
            Services.AddScoped<ITicketRepository, TicketRepository>();
            Services.AddScoped<IReportEvent, ReportEvent>();
            Services.AddScoped<IEventDetailViewQuery, EventDetailViewQuery>();
            Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            Services.AddScoped<IMemberOrganizationRepository, MemberOrganizationRepository>();
            Services.AddScoped<IPurchasedTicketRepository, PurchasedTicketRepository>();
            Services.AddScoped<ISupportChatRoomRepository, SupportChatRoomRepository>();
            Services.AddScoped<IMessageRepository, MessageRepository>();

            Services.AddScoped<IDapperHelper, DapperHelper>();
        }

    }
}
