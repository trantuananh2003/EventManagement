using EventManagement.Data.Helpers;
using EventManagement.Data.Models;
using EventManagement.Data.Queries.ModelDto;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IEventRepository : IRepository<Event>
    {
        void Update(Event entity);
        Task<(IEnumerable<T>, int)> GetEventsForOrganization<T>(string idOrganization, 
            string searchString, bool isUpComing, string status, 
            int pageSize, int pageIndex);

        Task<(IEnumerable<T>, int)> GetListHomeEvent<T>(
            string searchString, string fromDate, string toDate,
            int pageNumber, int pageSize);

        //Task<PagedList<HomeEventDto>> GetListHomeEvent(
        //    string searchString, DateTime fromDate, DateTime toDate,
        //    int pageNumber, int pageSize);
    }
}
