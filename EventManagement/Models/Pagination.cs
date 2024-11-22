namespace EventManagement.Models
{
    public class Pagination
    {
        public int CurrentPage { set; get; }
        public int PageSize { get; set; }
        public int TotalRecords {get;set;}
    }
}
