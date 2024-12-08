using EventManagement.Data.Queries;
using EventManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportEventController : ControllerBase
    {
        private readonly IReportEvent _reportEvent;
        private readonly ApiResponse _apiResponse;
        public ReportEventController(IReportEvent reportEvent) {
            _reportEvent = reportEvent;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("/GetTicketStatics")]
        public async Task<ActionResult<ApiResponse>> GetTicketStatics(string eventId)
        {
            var ticketStatistics = await _reportEvent.GetTicketStatisticsAsync(eventId);
            var totalOrder = await _reportEvent.GetTotalOrderAsync(eventId);

            var combinedReport = new
            {
                TicketStatistics = ticketStatistics,
                TotalOrder = totalOrder
            };

            _apiResponse.Result = combinedReport;
            return Ok(_apiResponse);
        }

        [HttpGet("TotalPaymentEvents")]
        public async Task<ActionResult<ApiResponse>> GetTotalPaymentEvent(string searchString,int pageNumber = 1, int pageSize = 5)
        {
            var pagedTotalPaymentEvent = await _reportEvent.GetTotalPaymentEvent(searchString, pageNumber, pageSize);

            Pagination pagination = new Pagination()
            {
                CurrentPage = pagedTotalPaymentEvent.CurrentNumber,
                PageSize = pagedTotalPaymentEvent.PageSize,
                TotalRecords = pagedTotalPaymentEvent.TotalCount
            };
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));

            return Ok(new ApiResponse
            {
                Result = pagedTotalPaymentEvent,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

    }
}
