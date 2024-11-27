using EventManagement.Common;
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
    [Route("api")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IPurchasedTicketService _purchasedTicketService;
        private readonly ApiResponse _apiResponse;

        public OrderController(IOrderService orderService, IPurchasedTicketService purchasedTicketService) {
            _orderService = orderService;
            _purchasedTicketService = purchasedTicketService;
            _apiResponse = new ApiResponse();
        }

        
        [HttpGet("[controller]Detail")]
        public async Task<ActionResult<ApiResponse>> GetOrderDetail([FromQuery] string idOrderHeader)
        {
            var listOrderDetail = await _orderService.GetOrderDetailDto(idOrderHeader);

            _apiResponse.Result = listOrderDetail;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }


        [HttpGet("[controller]Header/{orderHeaderId}")]
        public async Task<ActionResult<ApiResponse>> GetOrderHeaderById([FromRoute] string orderHeaderId)
        {
            var orderHeaderDto = await _orderService.GetOrderHeaderById(orderHeaderId);

            if (orderHeaderDto == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = orderHeaderDto;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        //Lay thong tin order cho user
        [HttpGet("user/{userId}/[controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllOrderByIdUser([FromRoute] string userId,
                string searchString, int pageSize = 0, int pageNumber = 1)
        {
            var (orderDto, totalRow) = await _orderService.GetAllOrderByIdUser(userId, searchString, pageSize, pageNumber);

            if (orderDto == null)
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

        //Lay thong tin order len cho to chuc
        [HttpGet("organization/{idOrganization}/[controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllOrderByIdOrganization([FromRoute] string idOrganization,
            string searchString, int pageSize = 0, int pageNumber = 1)
        {
            var (listResult, totalRow) = await _orderService.GetAllOrderByIdOrganization(idOrganization, searchString, pageSize, pageNumber);

            Pagination pagination = new Pagination
            {
                TotalRecords = totalRow,
                PageSize = pageSize,
                CurrentPage = pageNumber
            };
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));

            _apiResponse.Result = listResult;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        //Tao order
        [HttpPost("[controller]")]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto model)
        {
            var result = await _orderService.CreateOrder(model);

            if(result == EOrderCreate.OutOfStock)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add(EOrderCreate.OutOfStock.ToString());
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            else if(result == EOrderCreate.NotFoundItem )
            {
                _apiResponse.ErrorMessages.Add(EOrderCreate.NotFoundItem.ToString());
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
