using AutoMapper;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto;
using System.Net;

namespace EventManagement.Service.Organization
{
    public interface IOrganizationService
    {
        OrganizationResponse GetOrganization(string emailUser);

    }

    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _dbOrganization;
        private readonly IMapper _mapper;

        public OrganizationService(IOrganizationRepository dbOrganization, IMapper mapper) {
            _dbOrganization = dbOrganization;
            _mapper = mapper;
        }

        public OrganizationResponse GetOrganization(string idUser)
        {
            try
            {
                if (String.IsNullOrEmpty(idUser))
                {
                    return null;
                }

                var organization =  _dbOrganization.Get(u => u.IdUserOwner == idUser);
                var organizationReponse = _mapper.Map<OrganizationResponse>(organization);
                return organizationReponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
