using Azure.Storage.Blobs.Models;
using EventManagement.Common;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EventManagement.Controllers
{
    [Route("api/[Controller]")]
    public class SearchController : Controller
    {
        private ApiResponse _apiResponse;
        private readonly ISearchService _searchService;
        private readonly IEventService _eventService;

        public SearchController(ISearchService searchService, IEventService eventService)
        {
            _searchService = searchService;
            _eventService = eventService;
            _apiResponse = new ApiResponse();
        }   

        //Get list event for home page
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetListHomeEvent(DateTime fromDate, DateTime toDate,string searchString)
        {
            var pagedListHomeEvent = await _searchService.GetListHomeEvent(searchString, fromDate, toDate, 1,10);

            PaginationDto pagination = new PaginationDto()
            {
                CurrentPage = pagedListHomeEvent.CurrentPage,
                PageSize = pagedListHomeEvent.PageSize,
                TotalRecords = pagedListHomeEvent.TotalCount,
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedListHomeEvent));

            _apiResponse.Result = pagedListHomeEvent.Items;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpGet("{idEvent}")]
        public async Task<ActionResult<ApiResponse>> GetEventDetail([FromRoute] string idEvent)
        {
            var entity = await _eventService.GetEventById(idEvent);

            if (entity.Privacy == EPrivacy.Private.ToString())
            {
                _apiResponse.Result = SD.Privacy_Private;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }

            var eventDetailView = await _searchService.GetEventDetail(idEvent);

            _apiResponse.Result = eventDetailView;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

    }
}
