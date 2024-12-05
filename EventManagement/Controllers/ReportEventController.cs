using EventManagement.Data.Queries;
using EventManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet()]
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

    }
}
