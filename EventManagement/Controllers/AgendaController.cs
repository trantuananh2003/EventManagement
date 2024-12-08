using EventManagement.Models;
using EventManagement.Models.ModelsDto.AgendaDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : Controller
    {
        private readonly IAgendaService _agendaService;
        private readonly ApiResponse _apiResponse;

        public AgendaController(IAgendaService agendaService)
        {
            _agendaService = agendaService;
            _apiResponse = new ApiResponse();
        }

        // Lấy thông tin agenda dựa trên idAgenda
        [HttpGet("agenda/{idAgenda}", Name = "GetAgenda")]
        public async Task<ActionResult<ApiResponse>> Get(string idAgenda)
        {
            if (string.IsNullOrEmpty(idAgenda))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var agendaResult = await _agendaService.GetAgenda(idAgenda);

            if (agendaResult == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = agendaResult;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        // Tạo mới agenda
        [HttpPost("agenda")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] AgendaCreateDto model)
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

            var agendaDto = await _agendaService.CreateAgenda(model);
            _apiResponse.StatusCode = HttpStatusCode.Created;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = agendaDto;
            return CreatedAtRoute("GetAgenda", routeValues: new { idAgenda = agendaDto.IdAgenda }, value: _apiResponse);
        }

        // Cập nhật thông tin agenda
        [HttpPut("agenda")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> Put([FromBody] AgendaUpdateDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.IdAgenda) || !ModelState.IsValid)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { "Invalid agenda data." };
                return BadRequest(_apiResponse);
            }

            // Check Agenda exists
            var existingAgenda = await _agendaService.GetAgenda(model.IdAgenda);
            if (existingAgenda == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            await _agendaService.UpdateAgenda(model);
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
