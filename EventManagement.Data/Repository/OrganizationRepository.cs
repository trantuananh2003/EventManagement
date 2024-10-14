using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;

namespace EventManagement.Data.Repository
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        private readonly ApplicationDbContext _db;
        public OrganizationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Organization> UpdateAsync(Organization entity)
        {
            var existingEntity = await _db.Organizations.FindAsync(entity.IdOrganization);
            if (existingEntity != null)
            {
                existingEntity.NameOrganization = entity.NameOrganization;
                existingEntity.Description = entity.Description;
                existingEntity.City = entity.City;
                existingEntity.Country = entity.Country;
                _db.Organizations.Update(existingEntity);
                await SaveAsync();
            }
            return existingEntity;
        }
    }
}
