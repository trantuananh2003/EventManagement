using EventManagement.Common;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ApiResponse _apiResponse;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("[controller]/{idEvent}", Name = "GetEventById")]
        public async Task<ActionResult<ApiResponse>> Get(string idEvent)
        {
            if (string.IsNullOrEmpty(idEvent))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var eventResult = await _eventService.GetEventById(idEvent);

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

        // Lấy thông tin event dựa trên idOrganization
        [HttpGet("[controller]s", Name = "GetAll")]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string idOrganization, string searchString, string statusEvent,
            int pageSize = 0, int pageNumber = 1)
        {
            if(string.IsNullOrEmpty(idOrganization))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var (listEventDto, totalRow) = await _eventService.GetAllEvent(idOrganization, searchString, statusEvent,pageSize, pageNumber);

            if (listEventDto == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            Pagination pagination = new Pagination()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRow
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));

            _apiResponse.Result = listEventDto;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        // Tạo mới event
        [HttpPost("[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Policy = "AddEventPolicy")]
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
            _apiResponse.Result = new {eventId = eventDto.IdEvent };
            //return CreatedAtRoute("GetEventById", routeValues: new { idEvent = eventDto.IdEvent }, value: _apiResponse);
            return Ok(_apiResponse);
        }

        // Cập nhật thông tin event
        [HttpPut("[controller]/{idEvent}")]
        public async Task<ActionResult<ApiResponse>> Put([FromForm] EventUpdateDto model, [FromRoute] string idEvent)
        {
            model.IdEvent = idEvent;
            if (model == null || string.IsNullOrEmpty(model.IdEvent) || !ModelState.IsValid)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { "Invalid event data." };
                return BadRequest(_apiResponse);
            }

            // Check Event exists
            var existingEvent = await _eventService.GetEventById(model.IdEvent);
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

        [HttpPut("[controller]/{idEvent}/privacy")]
        public async Task<ActionResult<ApiResponse>> SetPrivacyEventByID([FromRoute] string idEvent,
            [FromBody] string privacy)
        {
            if(Enum.IsDefined(typeof(Privacy), privacy))
            {
                await _eventService.UpdatePrivacy(idEvent, privacy);
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
