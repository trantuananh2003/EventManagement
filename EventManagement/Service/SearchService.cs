using AutoMapper;
using EventManagement.Data.Helpers;
using EventManagement.Data.Queries;
using EventManagement.Data.Queries.ModelDto;
using EventManagement.Data.Repository.IRepository;
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
        private readonly IEventDetailViewQuery _eventDetailViewQuery;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchService(IMapper mapper, IUnitOfWork unitOfWork, IEventDetailViewQuery eventDetailViewQuery)
        {
            _unitOfWork = unitOfWork;
            _eventDetailViewQuery = eventDetailViewQuery;
            _mapper = mapper;
        }

        public async Task<PagedListDto<HomeEventDto>> GetListHomeEvent(
            string search, DateTime fromDate, DateTime toDate, int pageNumber, int pageSize)
        {
            if (fromDate == DateTime.MinValue)
                fromDate = DateTime.Now.Date; // Mặc định từ ngày hôm nay

            if (toDate == DateTime.MinValue)
                toDate = DateTime.Now.AddMonths(6).Date; // Mặc định đến hết 6 tháng sau

            var stringFromDate = fromDate.ToString("yyyy-MM-dd");
            var stringToDate = toDate.ToString("yyyy-MM-dd");

            var (result, totalRecord) = await _unitOfWork.EventRepository.GetListHomeEvent<HomeEventDto>(search, stringFromDate, stringToDate, pageNumber, pageSize);
            PagedListDto<HomeEventDto> pagedHomeEventDto = new PagedListDto<HomeEventDto>
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Items = result,
                TotalCount = totalRecord,
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
