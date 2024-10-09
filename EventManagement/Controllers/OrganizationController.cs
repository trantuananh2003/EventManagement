using Azure;
using EventManagement.Models;
using EventManagement.Service.Organization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly ApiResponse _apiResponse;

        public OrganizationController(IOrganizationService organizationService) {
            _organizationService = organizationService;
            _apiResponse = new ApiResponse();
        }

        [HttpGet("getorganization/{id}")]
        public async Task<ActionResult<ApiResponse>> GetOrganization(string id)
        {
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }

                var organization = _organizationService.GetOrganization(id);

                if (organization == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Result = organization;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<String>() { ex.ToString() };
                return BadRequest(_apiResponse);
            }
        }
    }
}
