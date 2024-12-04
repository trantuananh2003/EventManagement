using EventManagement.Data.Models;

namespace EventManagement.Models.ModelsDto.EventDtos
{
    public class EventDto
    {
        public string IdEvent { get; set; }
        public string NameEvent { get; set; }
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Location { get; set; }
        public string Coordinates { get; set; }
        public string Privacy { get; set; }
        public string Status { get; set; }
        public string EventType { get; set; }
    }
}
