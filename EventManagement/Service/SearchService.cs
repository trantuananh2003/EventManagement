using AutoMapper;
using EventManagement.Data.Queries;
using ModelApi = EventManagement.Models.ModelQueries;
using ModelData = EventManagement.Data.Queries.ModelDto;


namespace EventManagement.Service
{
    public interface ISearchService
    {
        Task<IEnumerable<ModelApi.SearchItemDto.HomeEventDto>> GetListHomeEvent(string search);
        Task<ModelApi.EventDetailViewDto> GetEventDetail(string idEvent);

    }

    public class SearchService : ISearchService
    {
        private readonly ISearchQuery _searchQuery;
        private readonly IEventDetailViewQuery _eventDetailViewQuery;
        private readonly IMapper _mapper;


        public SearchService(IMapper mapper, ISearchQuery searchQuery, IEventDetailViewQuery eventDetailViewQuery)
        {
            _searchQuery = searchQuery;
            _eventDetailViewQuery = eventDetailViewQuery;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModelApi.SearchItemDto.HomeEventDto>> GetListHomeEvent(string search)
        {
            var listHomeEventFromData = await _searchQuery.GetListHomeEvent();
            var listHomeEventFromService = _mapper.Map<List<ModelApi.SearchItemDto.HomeEventDto>>(listHomeEventFromData);
            return listHomeEventFromService;
        }

        public async Task<ModelApi.EventDetailViewDto> GetEventDetail(string idEvent)
        {
            var eventDetailFromData = await _eventDetailViewQuery.GetEventDetailView(idEvent);
            var eventDetailFromService = _mapper.Map<ModelApi.EventDetailViewDto>(eventDetailFromData);
            return eventDetailFromService;
        }
    }
}
