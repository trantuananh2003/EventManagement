using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Service.OrganizationService
{
    public interface IOrganizationService
    {
        Task<OrganizationDto> GetOrganizationByIdUser(string idUser);

        Task CreateOrganization(OrganizationCreateDto modelRequest);

        Task UpdateOrganization(OrganizationUpdateDto modelRequest);

        Task<OrganizationDto> GetOrganizationById(string id);
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

        public async Task<OrganizationDto> GetOrganizationByIdUser(string idUser)
        {
            if (string.IsNullOrEmpty(idUser))
            {
                return null;
            }

            var organizationEntity = await _dbOrganization.GetAsync(u => u.IdUserOwner == idUser);
            var organizationResponse = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationResponse;
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
    }
}
