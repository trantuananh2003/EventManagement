using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.ModelsDto.AgendaDtos;
using System;
using System.Threading.Tasks;

namespace EventManagement.Service
{
    public interface IAgendaService
    {
        Task<AgendaDto> GetAgenda(string idAgenda);
        Task<AgendaDto> CreateAgenda(AgendaCreateDto modelRequest);
        Task UpdateAgenda(AgendaUpdateDto modelRequest);
    }

    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _dbAgenda;
        private readonly IMapper _mapper;

        public AgendaService(IAgendaRepository dbAgenda, IMapper mapper)
        {
            _dbAgenda = dbAgenda;
            _mapper = mapper;
        }

        public async Task<AgendaDto> GetAgenda(string idAgenda)
        {
            if (string.IsNullOrEmpty(idAgenda))
            {
                return null;
            }

            var agendaEntity = await _dbAgenda.GetAsync(a => a.IdAgenda == idAgenda);
            if (agendaEntity == null)
            {
                return null;
            }

            var agendaResponse = _mapper.Map<AgendaDto>(agendaEntity);
            return agendaResponse;
        }

        public async Task<AgendaDto> CreateAgenda(AgendaCreateDto modelRequest)
        {
            var agendaEntity = _mapper.Map<Agenda>(modelRequest);
            agendaEntity.IdAgenda = Guid.NewGuid().ToString();
            await _dbAgenda.CreateAsync(agendaEntity);
            return _mapper.Map<AgendaDto>(agendaEntity);
        }

        public async Task UpdateAgenda(AgendaUpdateDto modelRequest)
        {
            var agendaEntity = _mapper.Map<Agenda>(modelRequest);
            await _dbAgenda.UpdateAsync(agendaEntity);
        }
    }
}
