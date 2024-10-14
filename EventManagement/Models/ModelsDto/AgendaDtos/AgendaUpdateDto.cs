using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.AgendaDtos
{
    public class AgendaUpdateDto
    {
        public string IdAgenda { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
    }
}
