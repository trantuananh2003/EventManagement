using Azure;
using EventManagement.Common;
using EventManagement.Data.Models;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrderHeaderDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
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
        private readonly IConfiguration _congifuration;
        private readonly ApiResponse _apiResponse;

        public OrderController(IOrderService orderService, 
            IPurchasedTicketService purchasedTicketService,
            IConfiguration configuration) {
            _orderService = orderService;
            _purchasedTicketService = purchasedTicketService;
            _congifuration = configuration;
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

        //Lay thong tin danh sach order cho user
        [HttpGet("user/{userId}/[controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllOrderByIdUser([FromRoute] string userId,
                string searchString, string statusFilter, int pageSize = 0, int pageNumber = 1)
        {
            var pagedList = await _orderService.GetAllOrderByIdUser(userId, searchString, statusFilter,
                pageSize, pageNumber);

            if (pagedList == null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            PaginationDto pagination = new PaginationDto
            {
                TotalRecords = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                CurrentPage = pagedList.CurrentNumber,
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = pagedList;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        //Lay thong tin order len cho to chuc
        [HttpGet("organization/{idOrganization}/[controller]s")]
        [Authorize(Policy = SD_Role_Permission.ManageOrderOverView)]
        public async Task<ActionResult<ApiResponse>> GetAllOrderByIdOrganization([FromRoute] string idOrganization,
            string searchString, int pageSize = 0, int pageNumber = 1)
        {
            var (listResult, totalRow) = await _orderService.GetAllOrderByIdOrganization(idOrganization, searchString, pageSize, pageNumber);

            PaginationDto pagination = new PaginationDto
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
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDto model )
        {
            var orderResult = await _orderService.CreateOrder(model);

            if(orderResult.eOrderCreate == EOrderCreate.OutOfStock)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add(EOrderCreate.OutOfStock.ToString());
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            else if(orderResult.eOrderCreate == EOrderCreate.NotFoundItem )
            {
                _apiResponse.ErrorMessages.Add(EOrderCreate.NotFoundItem.ToString());
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }
            //Tạo hóa đơn thành công
            else
            {
                StripeConfiguration.ApiKey = _congifuration["StripeSettings:SecretKey"];
                PaymentIntentCreateOptions options = new()
                {
                    Amount = (int)(orderResult.totalPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>
                      {
                        "card",
                      },
                };

                PaymentIntentService service = new();
                PaymentIntent response = service.Create(options);
                OrderConfirmDto orderConfirm = new OrderConfirmDto();
                orderConfirm.StripePaymentIntentId = response.Id;
                orderConfirm.ClientSecret = response.ClientSecret;
                orderConfirm.OrderHeaderId = orderResult.orderHeaderId;
                orderConfirm.TotalPrice = orderResult.totalPrice;
                orderConfirm.NumberPhone = model.NumberPhone;
                await _orderService.UpdateStatusPaymentOrder(orderResult.orderHeaderId, orderConfirm.StripePaymentIntentId, EStatusOrder.Pending);

                _apiResponse.Result = orderConfirm;
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                return Ok(_apiResponse);
            }
        }

        [HttpPut("confirm/[controller]/{orderHeaderId}")]
        public async Task<ActionResult<ApiResponse>> UpdateStatusOrder([FromRoute]string orderHeaderId, 
            [FromBody]OrderDataConfirm orderData)
        {
            if(!Enum.TryParse(orderData.Status, out EStatusOrder statusOrder))
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add("Fail to Update Status Order");
                return BadRequest(_apiResponse);
            }
            
            var result = await _orderService.UpdateStatusPaymentOrder(orderHeaderId, orderData.StripePaymentIntentId, statusOrder);
                _apiResponse.Result = result.ToString();

            if (result == EStatusOrder.Successful)
            {
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }

            _apiResponse.IsSuccess = false;
            return BadRequest(_apiResponse);
        }

        [HttpPost("retrieve-intent/{orderId}")]
        public async Task<ActionResult<ApiResponse>> RetrievePaymentIntent([FromRoute] string orderId)
        {
            var order = await _orderService.GetOrderHeaderById(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            if (string.IsNullOrEmpty(order.StripePaymentIntentId))
            {
                return BadRequest("Payment intent not created yet.");
            }

            StripeConfiguration.ApiKey = _congifuration["StripeSettings:SecretKey"];
            var service = new PaymentIntentService();
            try
            {
                // Lấy lại PaymentIntent từ Stripe
                var paymentIntent = await service.GetAsync(order.StripePaymentIntentId);

                // Kiểm tra trạng thái của PaymentIntent
                if (!(paymentIntent.Status == "requires_payment_method"))
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.ErrorMessages.Add("Payment failed. Please provide a new payment method.");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;

                    // Cập nhật trạng thái đơn hàng nếu cần
                    await _orderService.UpdateStatusPaymentOrder(order.IdOrderHeader, order.StripePaymentIntentId, EStatusOrder.Fail);

                    return BadRequest(_apiResponse);
                }

                // Nếu PaymentIntent hợp lệ để thanh toán
                OrderConfirmDto orderConfirm = new OrderConfirmDto
                {
                    StripePaymentIntentId = paymentIntent.Id,
                    ClientSecret = paymentIntent.ClientSecret,
                    OrderHeaderId = orderId,
                    TotalPrice = order.PriceTotal,
                    NumberPhone = order.NumberPhone,
                };

                _apiResponse.IsSuccess = true;
                _apiResponse.Result = orderConfirm;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages.Add($"Error retrieving payment intent: {ex.Message}");
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode(500, _apiResponse);
            }
        }
    }
}
