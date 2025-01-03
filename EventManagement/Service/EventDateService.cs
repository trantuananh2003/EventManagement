using AutoMapper;
using EventManagement.App.Models.ModelsDto.EventDateDtos;
using EventManagement.Data.Models;
using EventManagement.Data.Repository;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.EventDateDtos;

namespace EventManagement.Service
{
    public interface IEventDateService
    {
        Task<List<EventDateDto>> GetAllEventDate(string idEvent);
        Task CreateEventDate(EventDateCreateDto entity, string idEvent);
        Task UpdateEventDate(EventDateUpdateDto modelDto, string eventDateId);
        Task RemoveEventDate(string eventDateId);
        Task SaveAllEventDate(List<EventDateSaveDto> listEventDateSave, List<EventDateSaveDto> listEventDateDelete, string idEvent);
        Task<bool> CheckAlreadyExistTicketReference(List<EventDateSaveDto> listEventDateDelete);
    }

    public class EventDateService : IEventDateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventDateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EventDateDto>> GetAllEventDate(string idEvent)
        {
            IEnumerable<EventDate> eventDateEntity = await _unitOfWork.EventDateRepository.GetAllAsync(filter: e => e.EventId == idEvent);
            var listEventDate = _mapper.Map<List<EventDateDto>>(eventDateEntity);
            return listEventDate;
        }

        public async Task<bool> CheckAlreadyExistTicketReference(List<EventDateSaveDto> listEventDateDelete)
        {
            var listEventDateDeleteEntity = _mapper.Map<List<EventDate>>(listEventDateDelete);

            foreach (var eventDate in listEventDateDeleteEntity)
            {
                var hasTickets = await _unitOfWork.TicketRepository.AnyAsync(ticket => ticket.EventDateId == eventDate.IdEventDate);

                if (hasTickets)
                {
                    return true; 
                }
            }

            return false;
        }

        public async Task SaveAllEventDate(List<EventDateSaveDto> listEventDateSave, List<EventDateSaveDto> listEventDateDelete, string idEvent)
        {

            var listEventDateEntity = _mapper.Map<List<EventDate>>(listEventDateSave);
            var listEventDateDeleteEntity = _mapper.Map<List<EventDate>>(listEventDateDelete);

            _unitOfWork.EventDateRepository.RemoveRange(listEventDateDeleteEntity);

            foreach (var item in listEventDateEntity)
            {
                if (string.IsNullOrEmpty(item.IdEventDate))
                {
                    item.IdEventDate = Guid.NewGuid().ToString();
                    await _unitOfWork.EventDateRepository.CreateAsync(item);
                }
                else
                {
                    _unitOfWork.EventDateRepository.Update(item);
                }
            }

            await _unitOfWork.SaveAsync();
        }

        #region UnUpdate
        public async Task<EventDateDto> GetEventDate(string idEventDate)
        {
            var entity = await _unitOfWork.EventDateRepository.GetAsync(x => x.IdEventDate == idEventDate);
            var model = _mapper.Map<EventDateDto>(entity);
            return model;
        }

        public async Task CreateEventDate(EventDateCreateDto modelDto, string idEvent)
        {
            EventDate entity = _mapper.Map<EventDate>(modelDto);
            await _unitOfWork.EventDateRepository.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateEventDate(EventDateUpdateDto modelDto, string eventDateId)
        {
            EventDate entity = _mapper.Map<EventDate>(modelDto);
            await _unitOfWork.EventDateRepository.CreateAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        async Task IEventDateService.RemoveEventDate(string eventDateId)
        {
            var entity = await _unitOfWork.EventDateRepository.GetAsync(x => x.IdEventDate == eventDateId);
            _unitOfWork.EventDateRepository.Remove(entity);
            await _unitOfWork.SaveAsync();
        }
        #endregion
    }
}
