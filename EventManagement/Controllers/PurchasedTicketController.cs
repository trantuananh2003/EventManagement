using EventManagement.Data.Repository.IRepository;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.PurchasedDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasedTicketController : Controller
    {
        private readonly IPurchasedTicketService _purchasedTicketService;
        private readonly ApiResponse _apiResponse;

        public PurchasedTicketController(IPurchasedTicketService purchasedTicketService)
        {
            _purchasedTicketService = purchasedTicketService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllPurchasedTicket(string idOrderHeader, string searchString, string status,
            int pageSize = 0, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(idOrderHeader))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            var (listPurchasedTicketDto, totalRow) = await _purchasedTicketService.GetAllPurchasedTicket(idOrderHeader, searchString, status, pageSize, pageNumber);

            if (listPurchasedTicketDto == null)
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

            _apiResponse.Result = listPurchasedTicketDto;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpGet("{idPurchasedTicket}")]
        public async Task<ActionResult<ApiResponse>> GetPurchasedTicketById(string idPurchasedTicket)
        {
            var purchasedTicketDto = await _purchasedTicketService.GetPurchasedTicketById(idPurchasedTicket);

            if (purchasedTicketDto == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = purchasedTicketDto;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpPut("{idPurchasedTicket}")]
        public async Task<ActionResult<ApiResponse>> UpdatePurchasedTicket(string idPurchasedTicket, [FromBody] PurchasedTicketUpdateDto model)
        {
            if (string.IsNullOrEmpty(idPurchasedTicket))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            await _purchasedTicketService.UpdatePurchasedTicket(idPurchasedTicket, model);

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
