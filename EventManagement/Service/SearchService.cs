using AutoMapper;
using EventManagement.Data.Helpers;
using EventManagement.Data.Queries;
using EventManagement.Data.Queries.ModelDto;
using EventManagement.Models;
using ModelApi = EventManagement.Models.ModelQueries;


namespace EventManagement.Service
{
    public interface ISearchService
    {
        Task<PagedListDto<HomeEventDto>> GetListHomeEvent(
            string search, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize);
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

        public async Task<PagedListDto<HomeEventDto>> GetListHomeEvent(
            string search, DateTime fromDate, DateTime toDate , int pageNumber, int pageSize)
        {
            // Kiểm tra nếu từ ngày hoặc đến ngày là DateTime.MinValue, thay đổi chúng thành giá trị mặc định
            if (fromDate == DateTime.MinValue)
                fromDate = DateTime.Now.Date; // Mặc định từ ngày hôm nay

            if (toDate == DateTime.MinValue)
                toDate = DateTime.Now.AddMonths(6).Date; // Mặc định đến hết 6 tháng sau

            var pagedHomeEvent = await _searchQuery.GetListHomeEvent(search, fromDate, toDate ,pageNumber, pageSize);
            PagedListDto<HomeEventDto> pagedHomeEventDto = new PagedListDto<HomeEventDto>
            {
                PageSize = pagedHomeEvent.PageSize,
                CurrentPage = pagedHomeEvent.CurrentNumber,
                Items = pagedHomeEvent,
                TotalCount = pagedHomeEvent.TotalCount,
            };
            return pagedHomeEventDto;
        }

        public async Task<ModelApi.EventDetailViewDto> GetEventDetail(string idEvent)
        {
            var eventDetailFromData = await _eventDetailViewQuery.GetEventDetailView(idEvent);
            var eventDetailFromService = _mapper.Map<ModelApi.EventDetailViewDto>(eventDetailFromData);
            return eventDetailFromService;
        }
    }
}
