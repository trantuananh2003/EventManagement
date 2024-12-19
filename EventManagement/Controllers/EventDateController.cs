using EventManagement.App.Models.ModelsDto.EventDateDtos;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDateDtos;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/")]
    public class EventDateController : Controller
    {
        private readonly IEventDateService _eventDateService;
        private readonly ApiResponse _apiResponse;

        public EventDateController(IEventDateService eventDateService)
        {
            _eventDateService = eventDateService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("[controller]s")]
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

        [HttpPut("[controller]")]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] EventDateCombineSaveDto eventDateCombine, [FromQuery] string idEvent)
        {
            if(await _eventDateService.CheckAlreadyExistTicketReference(eventDateCombine.ListEventDateDelete))
            {
                _apiResponse.StatusCode = HttpStatusCode.Conflict;
                _apiResponse.ErrorMessages.Add("Already Ticket take EventDate");
                _apiResponse.IsSuccess = false;
                return Conflict(_apiResponse);
            }

            await _eventDateService.SaveAllEventDate(eventDateCombine.ListEventDateDto, eventDateCombine.ListEventDateDelete, idEvent);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        #region unupdate

        [HttpPost("[controller]")]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] EventDateCreateDto modelCreateDto,[FromQuery] string idEvent)
        {
            await _eventDateService.CreateEventDate(modelCreateDto, idEvent);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpPut("[controller]/{eventDateId}")]
        public async Task<ActionResult<ApiResponse>> Put([FromBody] EventDateUpdateDto modelUpdateDto, [FromRoute] string eventDateId)
        {
            await _eventDateService.UpdateEventDate(modelUpdateDto, eventDateId);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpDelete("[controller]/{idEventDate}")]
        public async Task<ActionResult<ApiResponse>> Delete([FromRoute] string idEventDate)
        {
            await _eventDateService.RemoveEventDate(idEventDate);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        #endregion
    }
}
