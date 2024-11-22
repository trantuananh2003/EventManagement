using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.EventDateDtos;

namespace EventManagement.Service
{
    public interface IEventDateService
    {
        public Task<List<EventDateDto>> GetAllEventDate(string idEvent);

        public Task SaveAllEventDate(List<EventDateSaveDto> listEventDateSave, List<EventDateSaveDto> listEventDateDelete, string idEvent);
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
            if (idEvent == null)
            {
                return null;
            }

            IEnumerable<EventDate> eventDateEntity = await _dbEventDate.GetAllAsync(e => e.EventId == idEvent);
            var listEventDate = _mapper.Map<List<EventDateDto>>(eventDateEntity);
            return listEventDate;
        }

        public async Task SaveAllEventDate(List<EventDateSaveDto> listEventDateSave, List<EventDateSaveDto> listEventDateDelete, string idEvent)
        {

            var listEventDateEntity = _mapper.Map<List<EventDate>>(listEventDateSave);
            var listEventDateDeleteEntity = _mapper.Map<List<EventDate>>(listEventDateDelete);
            using (var transaction = _dbEventDate.BeginTransaction())
            {
                try
                {
                    _dbEventDate.RemoveRange(listEventDateDeleteEntity);

                    foreach (var item in listEventDateEntity)
                    {
                        if (string.IsNullOrEmpty(item.IdEventDate))
                        {
                            item.IdEventDate = Guid.NewGuid().ToString();
                            await _dbEventDate.CreateAsync(item);
                        }
                        else
                        {
                            _dbEventDate.Update(item);
                        }
                    }

                    await _dbEventDate.SaveAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("An error occurred while saving the entity.", ex);
                }
            }

        }
    }
}
