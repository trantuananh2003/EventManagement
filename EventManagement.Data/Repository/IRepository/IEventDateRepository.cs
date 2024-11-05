using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IEventDateRepository : IRepository<EventDate>
    {
        Task<EventDate> UpdateAsync(EventDate entity);

        Task<bool> SaveAllList(List<EventDate> listItem, string id);
    }
}
