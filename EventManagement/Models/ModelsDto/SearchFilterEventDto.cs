namespace EventManagement.Models.ModelsDto
{
    public class SearchFilterEventDto
    {
        public string SearchString { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
    }
}
