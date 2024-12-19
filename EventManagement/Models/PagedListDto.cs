namespace EventManagement.Models
{
    public class PagedListDto<T> 
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
