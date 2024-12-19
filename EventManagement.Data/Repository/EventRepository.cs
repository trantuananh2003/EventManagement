using Dapper;
using EventManagement.Data.Dapper;
using EventManagement.Data.DataConnect;
using EventManagement.Data.Helpers;
using EventManagement.Data.Models;
using EventManagement.Data.Queries.ModelDto;
using EventManagement.Data.Repository.IRepository;

namespace EventManagement.Data.Repository
{

    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IDapperHelper _dapper;

        public EventRepository(ApplicationDbContext db, IDapperHelper dapper) : base(db) {
            _db = db;
            _dapper = dapper;
        }

        public void Update(Event entity)
        {
            _db.Events.Update(entity);
        }

        public async Task<(IEnumerable<T>, int)> GetEventsForOrganization<T>(string idOrganization, string searchString, bool isUpComing ,string status, int pageSize, int pageIndex)
        {
            if(pageSize == 0)
                pageSize = 10;


            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("idOrganization", idOrganization, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("status", status, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("searchString", searchString, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("isUpComing", isUpComing, System.Data.DbType.Boolean, System.Data.ParameterDirection.Input);
            parameters.Add("pageSize", pageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("pageIndex", pageIndex, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("totalRecords", 0, System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var result = await _dapper.ExecuteStoreProcedureReturnList<T>("spGetAllEventForOrganization", parameters);
            var totalRecord = parameters.Get<int>("totalRecords");

            return (result, totalRecord);
        }


        public async Task<(IEnumerable<T>, int)> GetListHomeEvent<T>(
        string searchString, string fromDate, string toDate,
        int pageIndex, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 10;

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("searchString", searchString, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("fromDate", fromDate, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            parameters.Add("toDate", toDate, System.Data.DbType.String, System.Data.ParameterDirection.Input);

            parameters.Add("pageSize", pageSize, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("pageIndex", pageIndex, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
            parameters.Add("totalRecords", 0, System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var result = await _dapper.ExecuteStoreProcedureReturnList<T>("spGetListHomeEvent", parameters);
            var totalRecord = parameters.Get<int>("totalRecords");

            return (result, totalRecord);
        }

        //public async Task<PagedList<HomeEventDto>> GetListHomeEvent(
        //   string searchString, DateTime fromDate, DateTime toDate,
        //   int pageNumber, int pageSize)
        //{
        //    var query = from e in _db.Events
        //                join ed in _db.EventDates on e.IdEvent equals ed.EventId
        //                join t in _db.Tickets
        //                    on new { EventId = e.IdEvent, EventDateId = ed.IdEventDate }
        //                    equals new { t.EventId, t.EventDateId } into ticketGroup // Left join
        //                from t in ticketGroup.DefaultIfEmpty() // Nếu không có vé, vẫn lấy sự kiện
        //                where ed.ScheduledDate.Date >= fromDate.Date && ed.ScheduledDate.Date <= toDate.Date
        //                      && (string.IsNullOrEmpty(searchString) || e.NameEvent.Contains(searchString))
        //                      || (string.IsNullOrEmpty(searchString) || e.Location.Contains(searchString))
        //                group new { e, ed, t } by e.IdEvent into g // Nhóm chỉ dựa trên IdEvent
        //                select new HomeEventDto
        //                {
        //                    EventId = g.Key,
        //                    EventName = g.First().e.NameEvent, // Lấy thông tin từ một phần tử bất kỳ trong nhóm
        //                    UrlImage = g.First().e.UrlImage,
        //                    Location = g.First().e.Location,
        //                    NearDate = g.Min(x => x.ed.ScheduledDate).ToString("dd-MM-yyyy"), // Ngày gần nhất
        //                    PriceLow = g.Where(x => x.t != null).Min(x => (int?)x.t.Price) ?? 0, // Giá thấp nhất trong nhóm
        //                    PriceHigh = g.Where(x => x.t != null).Max(x => (int?)x.t.Price) ?? 0// Giá cao nhất trong nhóm
        //                };

        //    // Trả về kết quả phân trang
        //    var homeEventDtos = await PagedList<HomeEventDto>.ToPagedList(query, pageNumber, pageSize);

        //    return homeEventDtos;
        //}
    }
}
