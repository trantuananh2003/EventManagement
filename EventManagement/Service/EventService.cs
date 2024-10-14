using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.EventDtos;

namespace EventManagement.Service.EventService
{
    public interface IEventService
    {
        Task<EventDto> GetEvent(string idEvent);
        Task<EventDto> CreateEvent(EventCreateDto modelRequest);
        Task UpdateEvent(EventUpdateDto modelRequest);
    }

    public class EventService : IEventService
    {
        private readonly IEventRepository _dbEvent;
        private readonly IMapper _mapper;

        public EventService(IEventRepository dbEvent, IMapper mapper)
        {
            _dbEvent = dbEvent;
            _mapper = mapper;
        }

        // Lấy thông tin event dựa trên idOrganization
        public async Task<EventDto> GetEvent(string idEvent)
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

        // Tạo mới event
        public async Task<EventDto> CreateEvent(EventCreateDto modelRequest)
        {
            var eventEntity = _mapper.Map<Event>(modelRequest);
            eventEntity.IdEvent = Guid.NewGuid().ToString(); // Tạo IdEvent mới
            await _dbEvent.CreateAsync(eventEntity); // Lưu event mới vào database
            return _mapper.Map<EventDto>(eventEntity);
        }

        // Cập nhật thông tin event
        public async Task UpdateEvent(EventUpdateDto modelRequest)
        {
            var eventEntity = _mapper.Map<Event>(modelRequest);
            await _dbEvent.UpdateAsync(eventEntity);
        }
    }
}
