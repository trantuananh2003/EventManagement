using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;

namespace EventManagement.Data.Repository
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(Organization entity)
        {
            _db.Update(entity);
        }
    }
}
