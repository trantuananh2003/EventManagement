namespace EventManagement.Models
{
    //Object để phân trang
    public class PaginationDto
    {
        public int CurrentPage { set; get; }
        public int PageSize { get; set; }
        public int TotalRecords {get;set;}
    }
}
