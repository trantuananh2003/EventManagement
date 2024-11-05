namespace EventManagement.Models.ModelsDto.EventDateDtos
{
    public class EventDateSaveDto
    {
        public string EventId { get; set; }
        public string DateTitle { get; set; }
        public DateTime ScheduledDate { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
