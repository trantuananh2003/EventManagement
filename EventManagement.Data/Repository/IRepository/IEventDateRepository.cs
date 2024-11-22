using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IEventDateRepository : IRepository<EventDate>
    {
        void Update(EventDate entity);
    }
}
