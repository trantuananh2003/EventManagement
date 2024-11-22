using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
namespace EventManagement.Data.Repository
{
    public class MemberOrganizationRepository : Repository<MemberOrganization>, IMemberOrganizationRepository
    {
        private ApplicationDbContext _db;
        public MemberOrganizationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MemberOrganization entity)
        {
            throw new NotImplementedException();
        }
    }
}
