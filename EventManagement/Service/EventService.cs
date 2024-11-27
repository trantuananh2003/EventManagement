using AutoMapper;
using EventManagement.Common;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.EventDtos;
using Microsoft.Extensions.Logging;

namespace EventManagement.Service
{
    public interface IEventService
    {
        Task<EventDto> GetEventById(string idEvent);
        Task<(List<EventDto>, int)> GetAllEvent(string idOrganization, string searchString, string statusEvent
            , int pageSize = 3, int pageNumber = 1);
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
            try
            {
                if (string.IsNullOrEmpty(idEvent))
                {
                    return null; // Kiểm tra nếu idEvent không hợp lệ
                }

                var eventEntity = await _dbEvent.GetAsync(e => e.IdEvent == idEvent);
                if (eventEntity == null)
                {
                    return null; // Không tìm thấy event
                }

                // Map từ entity Event sang DTO EventDto
                var eventResponse = _mapper.Map<EventDto>(eventEntity);
                return eventResponse;
            }
            catch (Exception ex)
            {
                // Có thể log lỗi ở đây nếu cần
                return null;
            }
        }

        //Lấy toàn bộ thẻ 
        public async Task<(List<EventDto>, int)> GetAllEvent(string idOrganization, string searchString, string statusEvent
            , int pageSize = 0, int pageNumber = 1)
        {
            IEnumerable<Event> eventList;
            eventList = await _dbEvent.GetAllAsync(
                u => u.OrganizationId == idOrganization
                     && (string.IsNullOrEmpty(statusEvent) || u.Status == statusEvent)
                     && (string.IsNullOrEmpty(searchString) || u.NameEvent.ToLower().Contains(searchString.ToLower())),
                pageSize: pageSize,
                pageNumber: pageNumber);


            int totalRow = await _dbEvent.CountAllAsync(u => u.OrganizationId == idOrganization
                     && (string.IsNullOrEmpty(statusEvent) || u.Status == statusEvent)
                     && (string.IsNullOrEmpty(searchString) || u.NameEvent.ToLower().Contains(searchString.ToLower())));

            List<EventDto> listEventDto = _mapper.Map<List<EventDto>>(eventList);
            return (listEventDto, totalRow);
        }

        // Tạo mới event
        public async Task<EventDto> CreateEvent(EventCreateDto modelRequest)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelRequest.File.FileName)}";

            var eventEntity = _mapper.Map<Event>(modelRequest);
            eventEntity.IdEvent = Guid.NewGuid().ToString(); // Tạo IdEvent mới
            eventEntity.Privacy = Privacy.Private.ToString();
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
