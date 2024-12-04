using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.EventDtos
{
    public class EventCreateDto
    {
        [Required]
        public string NameEvent { get; set; }
        [Required]
        public string OrganizationId { get; set; } 
        public string Description { get; set; }
        public IFormFile? File { get; set; }
        public string Location { get; set; }
        public string Coordinates { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string EventType { get; set; }
    }
}
