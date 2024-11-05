using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Service.OrganizationService
{
    public interface IOrganizationService
    {
        Task CreateOrganization(OrganizationCreateDto modelRequest);

        Task UpdateOrganization(OrganizationUpdateDto modelRequest);

        Task<OrganizationDto> GetOrganizationById(string id);
        Task<OrganizationDto> GetOrganizationByIdUser(string idUserOwner);

    }

    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _dbOrganization;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizationService(IOrganizationRepository dbOrganization, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _dbOrganization = dbOrganization;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Create organization
        public async Task CreateOrganization(OrganizationCreateDto modelRequest)
        {
            var modelOrganization = _mapper.Map<Organization>(modelRequest);
            modelOrganization.IdOrganization = Guid.NewGuid().ToString();
            await _dbOrganization.CreateAsync(modelOrganization);
        }

        public async Task UpdateOrganization(OrganizationUpdateDto modelRequest)
        {
            var modelOrganization = _mapper.Map<Organization>(modelRequest);
            await _dbOrganization.UpdateAsync(modelOrganization);
        }

        public async Task<OrganizationDto> GetOrganizationById(string id)
        {
            var organizationEntity = await _dbOrganization.GetAsync(u => u.IdOrganization == id);
            var organizationReponse = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationReponse;
        }

        public async Task<OrganizationDto> GetOrganizationByIdUser(string idUserOwner)
        {
            var organizationEntity = await _dbOrganization.GetAsync(u => u.IdUserOwner == idUserOwner);
            var organizationReponse = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationReponse;
        }
    }
}
