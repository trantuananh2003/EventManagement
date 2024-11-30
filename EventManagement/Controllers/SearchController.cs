using Azure.Storage.Blobs.Models;
using EventManagement.Common;
using EventManagement.Models;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<ActionResult<ApiResponse>> GetListHomeEvent()
        {
            var listHomeEvent = await _searchService.GetListHomeEvent("");

            _apiResponse.Result = listHomeEvent;
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
