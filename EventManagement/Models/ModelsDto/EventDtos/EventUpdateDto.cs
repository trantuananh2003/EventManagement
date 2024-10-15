using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.EventDtos
{
    public class EventUpdateDto
    {
        [Required]
        public string IdEvent { get; set; }
        [Required]
        public string NameEvent { get; set; }
        public string Description { get; set; }
        public IFormFile? File { get; set; }
        public string Location { get; set; }

        [Required]
        public string Status { get; set; }
        [Required]
        public string EventType { get; set; }
    }
}
