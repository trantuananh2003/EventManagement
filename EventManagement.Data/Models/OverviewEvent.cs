
namespace EventManagement.Data.Models
{
    public class OverviewEvent
    {
        public string IdOverviewEvent { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string TypeOverView { get; set; }
        public string Description { get; set; }
    }
}
