using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IEventRepository : IRepository<Event>
    {
        void Update(Event entity);
        Task<(IEnumerable<T>, int)> GetEventsForOrganization<T>(string idOrganization, 
            string searchString, bool isUpComing, string status, 
            int pageSize, int pageIndex);


    }
}
