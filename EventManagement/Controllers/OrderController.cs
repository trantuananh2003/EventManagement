using EventManagement.Data.Models;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ApiResponse _apiResponse;

        public OrderController(IOrderService orderService) {
            _orderService = orderService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllOrderByIdUser([FromQuery] string userId,
            string searchString,int pageSize = 0, int pageNumber = 1)
        {
            var (orderDto,totalRow) = await _orderService.GetAllOrderByIdUser(userId, searchString, pageSize, pageNumber);
           
            if(orderDto == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            Pagination pagination = new Pagination
            {
                TotalRecords = totalRow,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = orderDto;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto model)
        {
            var result = await _orderService.CreateOrder(model);

            if(result == EOrderCreate.OutOfStock)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Result = "Out of stock";
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            else if(result == EOrderCreate.NotFoundItem )
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }
            else
            {
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                return Ok(_apiResponse);
            }
        }
    }
}
