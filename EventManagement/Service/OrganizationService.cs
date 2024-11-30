using AutoMapper;
using Azure.Storage.Blobs.Models;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Service
{
    public interface IOrganizationService
    {
        Task CreateOrganization(OrganizationCreateDto modelRequest);
        Task UpdateOrganization(OrganizationUpdateDto modelRequest);

        Task<OrganizationDto> GetOrganizationById(string id);
        Task<OrganizationDto> GetOrganizationByIdUser(string idUserOwner);
        Task<List<OrganizationDto>> GetJoinedOrganizationsByIdUser(string userId);

        Task<ServiceResult> AddMember(string emailUser, string idOrganization);
        Task<(List<MemberOrganizationDto>, int)> GetAllMemberByIdOrganization(string idOrganization, string searchString, int pageSize, int pageNumber);
        Task KickMember(string memberId);
    }

    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _dbOrganization;
        private readonly IMemberOrganizationRepository _dbMemberOrganization;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private ServiceResult _serviceResult;

        public OrganizationService(IOrganizationRepository dbOrganization, IMemberOrganizationRepository dbMemberOrganization, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _dbOrganization = dbOrganization;
            _dbMemberOrganization = dbMemberOrganization;
            _mapper = mapper;
            _userManager = userManager;
            _serviceResult = new ServiceResult();
        }

        #region Organization
        public async Task CreateOrganization(OrganizationCreateDto modelRequest)
        {
            var modelOrganization = _mapper.Map<Organization>(modelRequest);
            modelOrganization.IdOrganization = Guid.NewGuid().ToString();
            await _dbOrganization.CreateAsync(modelOrganization);
            await _dbOrganization.SaveAsync();
        }

        public async Task UpdateOrganization(OrganizationUpdateDto modelRequest)
        {
            var modelOrganization = _mapper.Map<Organization>(modelRequest);
            await _dbOrganization.UpdateAsync(modelOrganization);
            await _dbOrganization.SaveAsync();
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
        public async Task<List<OrganizationDto>> GetJoinedOrganizationsByIdUser(string userId)
        {
            var listMemberOrganization = await _dbMemberOrganization.GetAllAsync(o => o.IdUser == userId, includeProperties: "Organization");
            var listOrganizationDto = new List<OrganizationDto>();
            foreach (var memberOrganization in listMemberOrganization)
            {
                listOrganizationDto.Add(_mapper.Map<OrganizationDto>(memberOrganization.Organization));
            }
            return listOrganizationDto;
        }
        #endregion

        #region Member Organization
        public async Task<(List<MemberOrganizationDto>, int)> GetAllMemberByIdOrganization(string idOrganization, string searchString, int pageSize, int pageNumber)
        {
            var listEntity = await _dbMemberOrganization.GetAllAsync(o => o.IdOrganization == idOrganization
                    && (string.IsNullOrEmpty(searchString) || o.User.FullName.ToLower().Contains(searchString.ToLower())
                    ), includeProperties: "User",
                    pageSize: pageSize,
                    pageNumber: pageNumber);

            int totalRow = await _dbMemberOrganization.CountAllAsync(o => o.IdOrganization == idOrganization
                    && (string.IsNullOrEmpty(searchString) || o.User.FullName.ToLower().Contains(searchString.ToLower())
                    ));

            var listDto = _mapper.Map<List<MemberOrganizationDto>>(listEntity);
            return (listDto, totalRow);
        }

        public async Task<ServiceResult> AddMember(string emailUser, string idOrganization)
        {
            var userEntity = await _userManager.FindByEmailAsync(emailUser);
            var entity = await _dbMemberOrganization.GetAsync(x => x.IdOrganization == idOrganization && x.IdUser == userEntity.Id);

            //Kiem tra trung thanh vien
            if(entity != null) //Co thanh vien
            {
                _serviceResult.IsSuccess = false;
                _serviceResult.Message.Add("Already user in your");
                return _serviceResult;
            }

            await _dbMemberOrganization.CreateAsync(new MemberOrganization
            {
                MemberId = Guid.NewGuid().ToString(),
                IdOrganization = idOrganization,
                IdUser = _userManager.FindByEmailAsync(emailUser).Result.Id
            });
            await _dbMemberOrganization.SaveAsync();

            _serviceResult.IsSuccess = true;
            return _serviceResult;
        }

        public async Task KickMember(string memberId)
        {
            var entity = await _dbMemberOrganization.GetAsync(o => o.MemberId == memberId);
            _dbMemberOrganization.Remove(entity);
            await _dbMemberOrganization.SaveAsync();
        }
        #endregion
    }
}
