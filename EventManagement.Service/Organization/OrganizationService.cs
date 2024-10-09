

namespace EventManagement.Service.Organization
{
    public interface IOrganizationService
    {
        OrganizationService GetOrganization();

    }

    public class OrganizationService : IOrganizationService
    {
        public OrganizationService() {
            
        }

        public OrganizationService GetOrganization()
        {
            throw new NotImplementedException();
        }
    }
}
