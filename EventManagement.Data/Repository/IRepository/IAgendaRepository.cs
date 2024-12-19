using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IAgendaRepository : IRepository<Agenda>
    {
        void Update(Agenda entity);
    }
}
