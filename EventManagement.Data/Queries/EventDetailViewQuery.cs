using EventManagement.Data.DataConnect;
using EventManagement.Data.Queries.ModelDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EventManagement.Data.Queries
{

    public interface IEventDetailViewQuery
    {
        Task<EventDetailViewDto> GetEventDetailView(string idEvent);
    }

    public class EventDetailViewQuery : IEventDetailViewQuery
    {
        private readonly ApplicationDbContext _db;

        public EventDetailViewQuery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<EventDetailViewDto> GetEventDetailView(string idEvent)
        {
            var eventDetailViewDto = await _db.Events.Where(e => e.IdEvent == idEvent)
                .GroupJoin(_db.Tickets,
                           e => e.IdEvent,
                           t => t.EventId,
                           (e, ticketGroup) => new
                           {
                               Event = e,
                               Tickets = ticketGroup
                           })
                .Select(eventGroup => new EventDetailViewDto
                {
                    EventId = eventGroup.Event.IdEvent,
                    EventName = eventGroup.Event.NameEvent,
                    UrlImage = eventGroup.Event.UrlImage,
                    Location = eventGroup.Event.Location,
                    Description = eventGroup.Event.Description,
                    NameOrganization = _db.Organizations.Where(o => o.IdOrganization == eventGroup.Event.OrganizationId)
                                        .Select(o => o.NameOrganization)
                                        .FirstOrDefault(),
                    Tickets = eventGroup.Tickets.Select(ticket => new TicketTimeViewDto
                    {
                        NameTicket = ticket.NameTicket,
                        ScheduledDate = _db.EventDates
                                       .Where(ed => ed.IdEventDate == ticket.EventDateId)
                                       .Select(ed => ed.ScheduledDate)
                                       .FirstOrDefault(),

                    }).ToList()
                }).FirstOrDefaultAsync();

            return eventDetailViewDto;
        }
    }
}
