using EventManagement.Data.Models;

namespace EventManagement.Models.ModelsDto.EventDateDtos
{
    public class EventDateDto
    {
        public string IdEventDate { get; set; }
        public string EventId { get; set; }
        public string DateTitle { get; set; }
        public string ScheduledDate { get; set; } // Change data type to string
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
