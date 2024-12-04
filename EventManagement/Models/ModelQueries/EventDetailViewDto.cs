namespace EventManagement.Models.ModelQueries
{
    public class EventDetailViewDto
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string UrlImage { get; set; }
        public string Location { get; set; }
        public string Coordinates { get; set; }
        public string Description { get; set; }
        public string OrganizationId { get; set; }
        public string NameOrganization { get; set; }
        public string UrlImageOrganization { get; set; }
        public string Status { get; set; }
        public string Privacy { get; set; }
        public string EventType { get; set; }
        public List<TicketTimeViewDto> Tickets { get; set; }

    }

    public class TicketTimeViewDto
    {
        public string NameTicket { get; set; }  //from Model Ticket
        public string ScheduledDate { get; set; } //from Model EventDate
    }
}
