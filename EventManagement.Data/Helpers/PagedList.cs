using EventManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentNumber = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = pageSize > 0
                ? await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
                : await source.ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
