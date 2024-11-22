using Azure;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly ApiResponse _apiResponse;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
            _apiResponse = new ApiResponse();
        }

        // Get organization of a user
        [HttpGet("[controller]/{userId}", Name = "GetOrganizationByIdUser")]
        public async Task<ActionResult<ApiResponse>> Get([FromRoute] string userId)
        {
            // Add delay of 3 seconds
            await Task.Delay(3000);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }



            var organization = await _organizationService.GetOrganizationByIdUser(userId);

            if (organization == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }

            return Ok(new ApiResponse
            {
                Result = organization,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpPost("[controller]")]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] OrganizationCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()
                });
            }

            await _organizationService.CreateOrganization(model);

            return CreatedAtRoute("GetOrganizationByIdUser", new { userId = model.IdUserOwner }, new ApiResponse
            {
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true
            });
        }

        [HttpPut("[controller]/{userId}")]
        public async Task<ActionResult<ApiResponse>> Put([FromRoute] string userId, [FromBody] OrganizationUpdateDto model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid organization data." }
                });
            }

            var existingOrganization = await _organizationService.GetOrganizationByIdUser(userId);
            if (existingOrganization == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }

            await _organizationService.UpdateOrganization(model);

            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpGet("[controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllByIdUser([FromQuery] string userId)
        {
            if(string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }

            var organizations = await _organizationService.GetJoinedOrganizationsByIdUser(userId);

            return Ok(new ApiResponse
            {
                Result = organizations,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }
    }
}
