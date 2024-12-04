using AutoMapper;
using EventManagement.Common;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDtos;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EventManagement.Service
{
    public interface IEventService
    {
        Task<EventDto> GetEventById(string idEvent);
        Task<PagedListDto<EventDto>> GetAllPagedEvent(string idOrganization, string searchString, string statusEvent, int pageSize = 0, int pageNumber = 1);
        Task<EventDto> CreateEvent(EventCreateDto modelRequest);
        Task UpdateEvent(EventUpdateDto modelRequest);
        Task UpdatePrivacy(string idEvent, string privacy);
    }

    public class EventService : IEventService
    {
        private readonly IEventRepository _dbEvent;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;

        public EventService(IEventRepository dbEvent, IMapper mapper, IBlobService blobService)
        {
            _dbEvent = dbEvent;
            _mapper = mapper;
            _blobService = blobService;
        }

        // Lấy thông tin event dựa trên idOrganization
        public async Task<EventDto> GetEventById(string idEvent)
        {
            var eventEntity = await _dbEvent.GetAsync(e => e.IdEvent == idEvent);
            if (eventEntity == null)
            {
                return null;
            }

            // Map từ entity Event sang DTO EventDto
            var eventResponse = _mapper.Map<EventDto>(eventEntity);
            return eventResponse;
            
        }

        //Lấy toàn bộ thẻ 
        public async Task<PagedListDto<EventDto>> GetAllPagedEvent(string idOrganization, 
            string searchString, string statusEvent, int pageSize = 0, int pageNumber = 1)
        {
            var pagedEvent = await _dbEvent.GetPagedAllAsync(
                filter: e => e.OrganizationId == idOrganization
                    && (string.IsNullOrEmpty(statusEvent) || e.Status == statusEvent)
                    && (string.IsNullOrEmpty(searchString) || e.NameEvent.ToLower().Contains(searchString.ToLower())),
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            var eventListDto = _mapper.Map<List<EventDto>>(pagedEvent);
            var pagedEventDto = new PagedListDto<EventDto>()
            {
                CurrentPage = pagedEvent.CurrentNumber,
                PageSize = pagedEvent.PageSize,
                TotalCount = pagedEvent.TotalCount,
                TotalPage = pagedEvent.TotalPage,
                Items = eventListDto
            };

            return pagedEventDto;
        }

        // Tạo mới event
        public async Task<EventDto> CreateEvent(EventCreateDto modelRequest)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelRequest.File.FileName)}";

            var eventEntity = _mapper.Map<Event>(modelRequest);
            eventEntity.IdEvent = Guid.NewGuid().ToString(); // Tạo IdEvent mới
            eventEntity.Privacy = EPrivacy.Private.ToString();
            eventEntity.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelRequest.File);

            await _dbEvent.CreateAsync(eventEntity); // Lưu event mới vào database
            await _dbEvent.SaveAsync(); // Lưu thay đổi vào database
            return _mapper.Map<EventDto>(eventEntity);
        }

        // Cập nhật thông tin event
        public async Task UpdateEvent(EventUpdateDto modelRequest)
        {
            //CHua xu ly viec mapping qua 
            var eventEntity = await _dbEvent.GetAsync(u => u.IdEvent == modelRequest.IdEvent);
            _mapper.Map(modelRequest, eventEntity);
            if (modelRequest.File != null && modelRequest.File.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelRequest.File.FileName)}";
                await _blobService.DeleteBlob(eventEntity.UrlImage.Split('/').Last(), SD.SD_Storage_Containter);
                eventEntity.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelRequest.File);
            }

            _dbEvent.Update(eventEntity);
            await _dbEvent.SaveAsync();
        }

        public async Task UpdatePrivacy(string idEvent, string privacy)
        {
            var entity = await _dbEvent.GetAsync(x => x.IdEvent == idEvent);
            entity.Privacy = privacy.ToString();
            _dbEvent.Update(entity);
            await _dbEvent.SaveAsync();
        }
    }
}
