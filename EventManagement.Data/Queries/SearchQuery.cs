using EventManagement.Data.DataConnect;
using EventManagement.Data.Queries.ModelDto;
using EventManagement.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Data.Queries
{
    public interface ISearchQuery
    {
        Task<PagedList<HomeEventDto>> GetListHomeEvent(
                    string searchString, DateTime fromDate, DateTime toDate,
                    int pageNumber, int pageSize);
    }

    public class SearchQuery : ISearchQuery
    {
        private readonly ApplicationDbContext _db;

        public SearchQuery(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<PagedList<HomeEventDto>> GetListHomeEvent(
            string searchString, DateTime fromDate, DateTime toDate,
            int pageNumber, int pageSize)
        {
            var query = from e in _db.Events
                        join ed in _db.EventDates on e.IdEvent equals ed.EventId
                        join t in _db.Tickets
                            on new { EventId = e.IdEvent, EventDateId = ed.IdEventDate }
                            equals new { t.EventId, t.EventDateId } into ticketGroup // Left join
                        from t in ticketGroup.DefaultIfEmpty() // Nếu không có vé, vẫn lấy sự kiện
                        where ed.ScheduledDate.Date >= fromDate.Date && ed.ScheduledDate.Date <= toDate.Date
                              && (string.IsNullOrEmpty(searchString) || e.NameEvent.Contains(searchString))
                              || (string.IsNullOrEmpty(searchString) || e.Location.Contains(searchString))
                        group new { e, ed, t } by e.IdEvent into g // Nhóm chỉ dựa trên IdEvent
                        select new HomeEventDto
                        {
                            EventId = g.Key,
                            EventName = g.First().e.NameEvent, // Lấy thông tin từ một phần tử bất kỳ trong nhóm
                            UrlImage = g.First().e.UrlImage,
                            Location = g.First().e.Location,
                            NearDate = g.Min(x => x.ed.ScheduledDate).ToString("dd-mm-yyyy"), // Ngày gần nhất
                            PriceLow = g.Where(x => x.t != null).Min(x => (int?)x.t.Price) ?? 0, // Giá thấp nhất trong nhóm
                            PriceHigh = g.Where(x => x.t != null).Max(x => (int?)x.t.Price) ?? 0// Giá cao nhất trong nhóm
                        };


            // Trả về kết quả phân trang
            var homeEventDtos = await PagedList<HomeEventDto>.ToPagedList(query, pageNumber, pageSize);

            return homeEventDtos;
        }
    }
}
