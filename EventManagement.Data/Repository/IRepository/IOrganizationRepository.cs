using EventManagement.Data.Models;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task Update(Organization entity);
    }
}
