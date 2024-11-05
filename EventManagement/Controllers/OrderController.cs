using EventManagement.Data.Models;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;
using EventManagement.Service;
using EventManagement.Service.OrganizationService;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto model)
        {
            //_orderService.CreateOrder(model);
            return Ok(_apiResponse);
        }
    }
}
