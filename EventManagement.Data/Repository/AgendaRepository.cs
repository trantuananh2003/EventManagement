using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;

namespace EventManagement.Data.Repository
{
    public class AgendaRepository : Repository<Agenda>, IAgendaRepository
    {
        private readonly ApplicationDbContext _db;

        public AgendaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Agenda entity)
        {
            _db.Agendas.Update(entity);
        }
    }
}
