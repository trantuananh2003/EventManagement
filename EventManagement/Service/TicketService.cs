
using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.TicketDtos;

namespace EventManagement.Service
{
    public interface ITicketService
    {
        Task<TicketDto> GetTicket(string idTicket);
        Task<List<TicketDto>> GetAllTicket(string idEvent);
        Task<TicketDto> CreateTicket(TicketCreateDto modelRequest);
        Task UpdateTicket(TicketUpdateDto modelRequest, string idTicket);
        Task DeleteTicket(string idTicket);
    }

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _dbTicket;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository dbTicket, IMapper mapper)
        {
            _dbTicket = dbTicket;
            _mapper = mapper;
        }

        public async Task<TicketDto> CreateTicket(TicketCreateDto modelRequest)
        {
            var ticketEntity = _mapper.Map<Ticket>(modelRequest);
            ticketEntity.IdTicket = Guid.NewGuid().ToString();
            await _dbTicket.CreateAsync(ticketEntity);
            return _mapper.Map<TicketDto>(ticketEntity);
        }

        public async Task<List<TicketDto>> GetAllTicket(string idEvent)
        {
            if (string.IsNullOrEmpty(idEvent))
            {
                return null;
            }

            var listTicketEntity = await _dbTicket.GetAllAsync(u => u.EventId == idEvent,includeProperties: "EventDate");
            return _mapper.Map<List<TicketDto>>(listTicketEntity);
        }

        public async Task<TicketDto> GetTicket(string idTicket)
        {
            if (string.IsNullOrEmpty(idTicket))
            {
                return null;
            }

            var ticketEntity = await _dbTicket.GetAsync(u => u.IdTicket == idTicket);
            return _mapper.Map<TicketDto>(ticketEntity);
        }

        public async Task UpdateTicket(TicketUpdateDto itemUpdate, string idTicket)
        {
            var ticketEntity = await _dbTicket.GetAsync(u => u.IdTicket == idTicket);
            if (ticketEntity == null)
            {
                throw new Exception("Ticket not found");
            }
            var modelUpdate = _mapper.Map<Ticket>(itemUpdate);
            await _dbTicket.UpdateAsync(modelUpdate);
        }

        public async Task DeleteTicket(string idTicket)
        {
            var ticketEntity = await _dbTicket.GetAsync(u => u.IdTicket == idTicket);
            await _dbTicket.RemoveAsync(ticketEntity);
        }
        
    }
}
