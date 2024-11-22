using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDateDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventDateController : Controller
    {
        private readonly IEventDateService _eventDateService;
        private readonly ApiResponse _apiResponse;

        public EventDateController(IEventDateService eventDateService)
        {
            _eventDateService = eventDateService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet(Name = "GetAllEventDate")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery]string idEvent)
        {
            if (string.IsNullOrEmpty(idEvent))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var eventDateResult = await _eventDateService.GetAllEventDate(idEvent);

            if (eventDateResult == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = eventDateResult;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Post([FromBody]EventDateCombineSaveDto eventDateCombine,[FromQuery] string idEvent)
        {
            await _eventDateService.SaveAllEventDate(eventDateCombine.ListEventDateDto, eventDateCombine.ListEventDateDelete, idEvent);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
