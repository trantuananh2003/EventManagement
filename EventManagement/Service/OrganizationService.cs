using AutoMapper;
using EventManagement.Common;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Models.ModelsDto;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using EventManagement.Service.OutService;
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
        Task<List<OrganizationDto>> GetAllOrganization();
        Task UpdateStatusOrganization(string organizationId, string status);
        Task<ServiceResult> AddMember(string emailUser, string idOrganization);
        Task<PagedListDto<MemberOrganizationDto>> GetAllMemberByIdOrganization(string idOrganization, string searchString, int pageSize, int pageNumber);
        Task KickMember(string memberId);
    }

    public class OrganizationService : IOrganizationService
    {
        private readonly IMemberOrganizationRepository _dbMemberOrganization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private ServiceResult _serviceResult;

        public OrganizationService(IOrganizationRepository dbOrganization, IMemberOrganizationRepository dbMemberOrganization, 
            IMapper mapper, UserManager<ApplicationUser> userManager, IBlobService blobService, IUnitOfWork unitOfWork)
        {
            _dbMemberOrganization = dbMemberOrganization;
            _mapper = mapper;
            _userManager = userManager;
            _serviceResult = new ServiceResult();
            _blobService = blobService;
            _unitOfWork = unitOfWork;
        }

        #region Organization
        public async Task CreateOrganization(OrganizationCreateDto modelRequest)
        {
            var modelOrganization = _mapper.Map<Organization>(modelRequest);
            modelOrganization.IdOrganization = Guid.NewGuid().ToString();

            await _unitOfWork.OrganizationRepository.CreateAsync(modelOrganization);
            await _unitOfWork.SaveAsync();

            var userEntity = await _userManager.FindByIdAsync(modelRequest.IdUserOwner);
            await AddMember(userEntity.Email, modelOrganization.IdOrganization);
        }

        public async Task UpdateOrganization(OrganizationUpdateDto modelUpdateDto)
        {
            var modelEntity = await _unitOfWork.OrganizationRepository.GetAsync(x => x.IdOrganization == modelUpdateDto.IdOrganization);
            var modelOrganization = _mapper.Map<Organization>(modelUpdateDto);

            if (modelUpdateDto.File != null && modelUpdateDto.File.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelUpdateDto.File.FileName)}";

                if (!string.IsNullOrEmpty(modelEntity.UrlImage))
                {
                    await _blobService.DeleteBlob(modelEntity.UrlImage.Split('/').Last(), SD.SD_Storage_Containter);
                }
                modelOrganization.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelUpdateDto.File);
            }

            await _unitOfWork.OrganizationRepository.Update(modelOrganization);
            await _unitOfWork.OrganizationRepository.SaveAsync();
        }

        public async Task<OrganizationDto> GetOrganizationById(string id)
        {
            var organizationEntity = await _unitOfWork.OrganizationRepository.GetAsync(u => u.IdOrganization == id);
            var organizationReponse = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationReponse;
        }

        public async Task<OrganizationDto> GetOrganizationByIdUser(string idUserOwner)
        {
            var organizationEntity = await _unitOfWork.OrganizationRepository.GetAsync(u => u.IdUserOwner == idUserOwner);
            var organizationReponse = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationReponse;
        }

        public async Task<List<OrganizationDto>> GetJoinedOrganizationsByIdUser(string userId)
        {
            var listMemberOrganization = await _unitOfWork.MemberOrganizationRepository.GetAllAsync(o => o.IdUser == userId, includeProperties: "Organization");
            var listOrganizationDto = new List<OrganizationDto>();
            foreach (var memberOrganization in listMemberOrganization)
            {
                listOrganizationDto.Add(_mapper.Map<OrganizationDto>(memberOrganization.Organization));
            }
            return listOrganizationDto;
        }


        public async Task<List<OrganizationDto>> GetAllOrganization()
        {
            var listOrganizaiton = await _unitOfWork.OrganizationRepository.GetAllAsync();
            var listOrganizationDto = _mapper.Map<List<OrganizationDto>>(listOrganizaiton);
            return listOrganizationDto;
        }

        public async Task UpdateStatusOrganization(string organizationId, string status)
        {
            var entity = await _unitOfWork.OrganizationRepository.GetAsync(x => x.IdOrganization == organizationId, tracked: true);
            entity.Status = status;
            await _unitOfWork.OrganizationRepository.SaveAsync();
        }
        #endregion

        #region Member Organization
        public async Task<PagedListDto<MemberOrganizationDto>> GetAllMemberByIdOrganization(string idOrganization, string searchString, int pageSize, int pageNumber)
        {
            var pagedMemberOrganization = await _dbMemberOrganization.GetPagedAllAsync(o => o.IdOrganization == idOrganization
                    && (string.IsNullOrEmpty(searchString) || o.User.FullName.ToLower().Contains(searchString.ToLower())
                    ), includeProperties: "User",
                    pageSize: pageSize,
                    pageNumber: pageNumber);

            var modelListDto = _mapper.Map<List<MemberOrganizationDto>>(pagedMemberOrganization);
            var pagedEventDto = new PagedListDto<MemberOrganizationDto>()
            {
                CurrentPage = pagedMemberOrganization.CurrentNumber,
                PageSize = pagedMemberOrganization.PageSize,
                TotalCount = pagedMemberOrganization.TotalCount,
                TotalPage = pagedMemberOrganization.TotalPage,
                Items = modelListDto,
            };

            return pagedEventDto;
        }

        public async Task<ServiceResult> AddMember(string emailUser, string idOrganization)
        {
            var userEntity = await _userManager.FindByEmailAsync(emailUser);

            if(userEntity == null)
            {
                _serviceResult.IsSuccess = false;
            }

            var entity = await _dbMemberOrganization.GetAsync(x => x.IdOrganization == idOrganization && x.IdUser == userEntity.Id);

            //Kiem tra trung thanh vien
            if(entity != null) //Co thanh vien
            {
                _serviceResult.IsSuccess = false;
                _serviceResult.Message.Add("Already user in your organization");
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
