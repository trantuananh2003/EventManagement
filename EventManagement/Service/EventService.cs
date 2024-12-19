using AutoMapper;
using EventManagement.Common;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Service.OutService;

namespace EventManagement.Service
{
    public interface IEventService
    {
        Task<EventDto> GetEventById(string idEvent);
        Task<PagedListDto<EventForOrganizationDto>> GetAllPagedEvent(string idOrganization, string searchString, string statusEvent, int pageSize = 0, int pageNumber = 1);
        Task<EventDto> CreateEvent(EventCreateDto modelRequest);
        Task UpdateEvent(EventUpdateDto modelRequest);
        Task UpdatePrivacy(string idEvent, string privacy);
    }

    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, IBlobService blobService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _blobService = blobService;
        }

        //lấy thông tin event dựa trên idOrganization
        public async Task<EventDto> GetEventById(string idEvent)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetAsync(e => e.IdEvent == idEvent);
            if (eventEntity == null)
            {
                return null;
            }

            var modelDto = _mapper.Map<EventDto>(eventEntity);
            return modelDto;
        }

        //lấy toàn bộ event
        public async Task<PagedListDto<EventForOrganizationDto>> GetAllPagedEvent(string idOrganization, 
            string searchString, string statusEvent, int pageSize = 0, int pageNumber = 1)
        {
            var (result, totalRecord) = await _unitOfWork.EventRepository.GetEventsForOrganization<EventForOrganizationDto>(idOrganization, searchString, 
                false, statusEvent, pageSize, pageNumber);

            var pagedEventDto = new PagedListDto<EventForOrganizationDto>()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalRecord,
                TotalPage = (int) Math.Ceiling(totalRecord / (double)pageSize),
                Items = result,
            };

            return pagedEventDto;
        }

        //tạo mới event
        public async Task<EventDto> CreateEvent(EventCreateDto modelRequest)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelRequest.File.FileName)}";

            var eventEntity = _mapper.Map<Event>(modelRequest);
            eventEntity.IdEvent = Guid.NewGuid().ToString();
            eventEntity.Privacy = EPrivacy.Private.ToString();
            eventEntity.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelRequest.File);

            await _unitOfWork.EventRepository.CreateAsync(eventEntity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }

        //cập nhật thông tin event
        public async Task UpdateEvent(EventUpdateDto modelRequest)
        {
            var eventEntity = await _unitOfWork.EventRepository.GetAsync(u => u.IdEvent == modelRequest.IdEvent);

            _mapper.Map(modelRequest, eventEntity);
            if (modelRequest.File != null && modelRequest.File.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelRequest.File.FileName)}";
                await _blobService.DeleteBlob(eventEntity.UrlImage.Split('/').Last(), SD.SD_Storage_Containter);
                eventEntity.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelRequest.File);
            }

            _unitOfWork.EventRepository.Update(eventEntity);
            await _unitOfWork.SaveAsync();
        }

        //cập nhập chính sách
        public async Task UpdatePrivacy(string idEvent, string privacy)
        {
            var entity = await _unitOfWork.EventRepository.GetAsync(x => x.IdEvent == idEvent);
            entity.Privacy = privacy.ToString();
            _unitOfWork.EventRepository.Update(entity);
            await _unitOfWork.SaveAsync();
        }
    }
}
