using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Service.EventService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Net;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ApiResponse _apiResponse;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
            _apiResponse = new ApiResponse();
        }

        // Lấy thông tin event dựa trên idOrganization
        [HttpGet("event/{idEvent}", Name = "GetEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Get(string idEvent)
        {
            if (string.IsNullOrEmpty(idEvent))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var eventResult = await _eventService.GetEvent(idEvent);

            if (eventResult == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = eventResult;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        // Tạo mới event
        [HttpPost("event")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Post([FromForm] EventCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(_apiResponse);
            }

            var eventDto = await _eventService.CreateEvent(model);
            _apiResponse.StatusCode = HttpStatusCode.Created;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = eventDto;
            return CreatedAtRoute("GetEvent", routeValues: new { idEvent = eventDto.IdEvent }, value: _apiResponse);
        }

        // Cập nhật thông tin event
        [HttpPut("event")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Put([FromForm] EventUpdateDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.IdEvent) || !ModelState.IsValid)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { "Invalid event data." };
                return BadRequest(_apiResponse);
            }

            // Check Event exists
            var existingEvent = await _eventService.GetEvent(model.IdEvent);
            if (existingEvent == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            await _eventService.UpdateEvent(model);
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
