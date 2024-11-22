using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IEventRepository : IRepository<Event>
    {
        void Update(Event entity);
    }
}
