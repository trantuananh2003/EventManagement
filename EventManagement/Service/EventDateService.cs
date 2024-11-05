using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.EventDateDtos;
using EventManagement.Service.EventService;
using Microsoft.IdentityModel.Tokens;

namespace EventManagement.Service
{
    public interface IEventDateService
    {
        public Task<List<EventDateDto>> GetAllEventDate(string idEvent);

        public Task<List<EventDateDto>> SaveAllEventDate(List<EventDateSaveDto> listEventDate, string idEvent);
    }

    public class EventDateService : IEventDateService
    {
        private readonly IEventDateRepository _dbEventDate;
        private readonly IMapper _mapper;

        public EventDateService(IEventDateRepository dbEvent, IMapper mapper)
        {
            _dbEventDate = dbEvent;
            _mapper = mapper;
        }

        public async Task<List<EventDateDto>> GetAllEventDate(string idEvent)
        {
            if(idEvent == null)
            {
                return null;
            }

            IEnumerable<EventDate> eventDateEntity = await _dbEventDate.GetAllAsync(e => e.EventId == idEvent);
            var listEventDate = _mapper.Map<List<EventDateDto>>(eventDateEntity);
            return listEventDate;
        }

        public async Task<List<EventDateDto>> SaveAllEventDate(List<EventDateSaveDto> listEventDate, string idEvent)
        {
            if (listEventDate.IsNullOrEmpty() && listEventDate.Count == 0)
            {
                return null;
            }

            var listEntity = _mapper.Map<List<EventDate>>(listEventDate);
            listEntity.ForEach(u => u.IdEventDate = Guid.NewGuid().ToString());
            var check = await _dbEventDate.SaveAllList(listEntity, idEvent);
            if(!check)
            {
                return null;
            }

            return _mapper.Map<List<EventDateDto>>(listEntity);
        }
    }
}
