﻿namespace EventManagement.Models.ModelsDto.AgendaDtos
{
    public class AgendaCreateDto
    {
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
