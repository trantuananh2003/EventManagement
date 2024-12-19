using EventManagement.Common;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("[controller]/{userId}", Name = "GetOrganizationByIdUser")]
        public async Task<ActionResult<ApiResponse>> Get([FromRoute] string userId)
        {

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
        public async Task<ActionResult<ApiResponse>> CreateOrganization([FromBody] OrganizationCreateDto model)
        {
            await _organizationService.CreateOrganization(model);

            return CreatedAtRoute("GetOrganizationByIdUser", new { userId = model.IdUserOwner }, new ApiResponse
            {
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true
            });
        }

        [HttpPut("[controller]/{userId}")]
        [Authorize(Policy = SD_Role_Permission.ManageOrganization_ClaimValue)]
        public async Task<ActionResult<ApiResponse>> Put([FromRoute] string userId, [FromForm] OrganizationUpdateDto modelUpdateDto)
        {
            if (modelUpdateDto == null )
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

            await _organizationService.UpdateOrganization(modelUpdateDto);

            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpGet("user/[controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllByIdUser([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
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

        [HttpGet("[controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllOrganization()
        {
            var listOrganization = await _organizationService.GetAllOrganization();
            return Ok(new ApiResponse
            {
                Result = listOrganization,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpPatch("status/[controller]/{organizationId}")]
        public async Task<ActionResult<ApiResponse>> ChangeStatus([FromRoute] string organizationId,[FromQuery] string status)
        {
            if (!Enum.IsDefined(typeof(EStatusOrganization), status))
            {
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }
            await _organizationService.UpdateStatusOrganization(organizationId, status.ToString());

            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
    