using EventManagement.Data.DataConnect;
using EventManagement.Data.Queries.ModelDto;

namespace EventManagement.Data.Queries
{
    public interface ISearchQuery
    {
        Task<List<HomeEventDto>> GetListHomeEvent();
    }

    public class SearchQuery : ISearchQuery
    {
        private readonly ApplicationDbContext _db;

        public SearchQuery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<HomeEventDto>> GetListHomeEvent()
        {
            var HomeEventDtos = _db.Events
                     .GroupJoin(
                         _db.Tickets,
                         e => e.IdEvent,
                         t => t.EventId,
                         (e, ticketGroup) => new
                         {
                             Event = e,
                             Tickets = ticketGroup
                         })
                     .AsEnumerable()
                     .Select(e => new HomeEventDto
                     {
                         EventId = e.Event.IdEvent,
                         EventName = e.Event.NameEvent,
                         UrlImage = e.Event.UrlImage,
                         Location = e.Event.Location,
                         PriceLow = e.Tickets.Any() ? e.Tickets.Min(t => t.Price) : 0,
                         PriceHigh = e.Tickets.Any() ? e.Tickets.Max(t => t.Price) : 0
                     })
                     .ToList();

            foreach (var homeEvent in HomeEventDtos)
            {
                Console.WriteLine("Name Evnet:" + homeEvent.EventName);
                Console.WriteLine("Price high" + homeEvent.PriceHigh);
                Console.WriteLine("Price low" + homeEvent.PriceLow);
            }

            return HomeEventDtos;
        }
    }
}
