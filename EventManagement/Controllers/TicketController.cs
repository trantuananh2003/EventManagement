using EventManagement.Common;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.EventDtos;
using EventManagement.Models.ModelsDto.TicketDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagement.Controllers
{
    [Route("api/[Controller]s")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private ApiResponse _apiResponse;
            
        public TicketController(ITicketService ticketService)
        { 
            _ticketService = ticketService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("{idTicket}")]
        public async Task<ActionResult<ApiResponse>> Get([FromRoute]string idTicket)
        {
            if (string.IsNullOrEmpty(idTicket))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var eventResult = await _ticketService.GetTicket(idTicket);

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

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] string idEvent)
        {
            var tickets = await _ticketService.GetAllTicket(idEvent);

            if (tickets == null || tickets.Count == 0)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = tickets;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] TicketCreateDto itemCreate)
        {
            var check = Enum.IsDefined(typeof(EStatusTicket), itemCreate.Status);
            Console.WriteLine(check);

            if (!ModelState.IsValid && !Enum.IsDefined(typeof(EStatusTicket), itemCreate.Status))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(_apiResponse);
            }

            var itemCreated = await _ticketService.CreateTicket(itemCreate);

            _apiResponse.IsSuccess = true;
            _apiResponse.Result = itemCreated;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPut("{idTicket}")]
        public async Task<ActionResult<ApiResponse>> Put([FromBody] TicketUpdateDto itemUpdate, [FromRoute] String idTicket)
        {
            if(itemUpdate == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            await _ticketService.UpdateTicket(itemUpdate, idTicket);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpDelete("{idTicket}")]
        public async Task<ActionResult<ApiResponse>> Delete([FromRoute] string idTicket)
        {
            if (string.IsNullOrEmpty(idTicket))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            await _ticketService.DeleteTicket(idTicket);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
    }
}
