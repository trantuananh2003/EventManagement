using EventManagement.Data.Models;
using EventManagement.Models;
using EventManagement.Models.ModelsDto.RoleDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Net;
using EventManagement.Middleware.Identity;
using EventManagement.Models.ModelsDto.OrganizationDtos;
using EventManagement.Common;
using System.Reflection;
using EventManagement.Data.DataConnect;
using System.Text.Json;
using EventManagement.Service;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationUserManager _userManager;
        private readonly ApiResponse _apiResponse;
        private readonly IOrganizationService _organizationService;
        private readonly ApplicationDbContext _db;

        public RoleController(RoleManager<ApplicationRole> roleManager, ApplicationUserManager userManager, 
            IOrganizationService organizationService, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _apiResponse = new ApiResponse();
            _userManager = userManager;
            _organizationService = organizationService;
            _db = db;
        }

        #region ManageRole
        [HttpGet("roles")]
        public async Task<ActionResult<ApiResponse>> GetAllRole([FromQuery] string organizationId)
        {
            _apiResponse.Result = await _roleManager.Roles.Where(r => r.OrganizationId == organizationId).ToListAsync();
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<ApiResponse>> GetDetailRole([FromRoute] string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var claims = await _roleManager.GetClaimsAsync(role);

            if (role == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                return NotFound(_apiResponse);
            }

            var roleDto = new RoleDto
            {
                RoleId = role.Id,
                NameRole = role.Name,
                Description = role.Description,
                ClaimValues = claims.Select(c => c.Value).ToArray()
            };

            _apiResponse.Result = roleDto;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpPost("role")]
        public async Task<ActionResult<ApiResponse>> AddRoleByOrganization([FromBody] RoleCreateDto roleCreateDto)
        {
            if (!ModelState.IsValid)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(ms => $"[{ms.Key}] : {ms.Value.Errors.FirstOrDefault()?.ErrorMessage}")
                    .ToList();
                return BadRequest(_apiResponse);
            }

            ApplicationRole role = new ApplicationRole
            {
                Name = roleCreateDto.NameRole,
                OrganizationId = roleCreateDto.OrganizationId
            };

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            foreach (var claim in roleCreateDto.ClaimValues)
            {
                var resultAddRole = await _roleManager.AddClaimAsync(role, new Claim(SD_Role_Permission.Organization_ClaimType, claim));
                if (!resultAddRole.Succeeded)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }
            }

            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpPut("role/{roleId}")]
        public async Task<ActionResult<ApiResponse>> UpdateRoleDetail([FromRoute] string roleId, [FromBody] RoleUpdateDto modelUpdate)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = modelUpdate.NameRole;
            role.Description = modelUpdate.Description;
            await _roleManager.UpdateAsync(role);

            var existingClaims = await _roleManager.GetClaimsAsync(role);

            // Xóa tất cả các claim hiện tại
            foreach (var claim in existingClaims)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
            }

            // Thêm lại các claim mới
            foreach (var newClaim in modelUpdate.ClaimValues)
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim(SD_Role_Permission.Organization_ClaimType, newClaim));
                if (!result.Succeeded)
                {
                    return BadRequest();
                }

            }

            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpDelete("role/{roleId}")]
        public async Task<ActionResult<ApiResponse>> DeleteRole([FromRoute] string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpGet("o-permissions")]
        public ActionResult<ApiResponse> GetAllOrganizationPermission()
        {
            // Lấy tất cả các hằng số từ lớp SD_Role_Permission
            var permissionValues = typeof(SD_Role_Permission)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy) // Chỉ lấy các hằng số public, static
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly) // Kiểm tra để đảm bảo đó là hằng số
                .Where(fi => !fi.Name.EndsWith("ClaimType"))
                .Select(fi => fi.GetValue(null)?.ToString()) // Lấy giá trị
                .ToArray(); // Chuyển thành mảng

            _apiResponse.Result = permissionValues;
            return Ok(_apiResponse);
        }
        #endregion

        #region ManageMember
        [HttpGet("members")]
        public async Task<ActionResult<ApiResponse>> GetMembers([FromQuery] string idOrganization, string searchString,
            int pageSize = 0, int pageNumber = 1)
        {
            var (listMember,totalRow) = await _organizationService.GetAllMemberByIdOrganization(idOrganization,searchString ,pageSize,pageNumber);

            Pagination pagination = new Pagination()
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRow
            };
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));

            return Ok(new ApiResponse
            {
                Result = listMember,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpPost("member")]
        public async Task<ActionResult<ApiResponse>> AddMember([FromBody] MemberOrganizationCreateDto model)
        {
            var serviceResult = await _organizationService.AddMember(model.EmailUser,model.IdOrganization);

            if (serviceResult.IsSuccess)
            {
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            else
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = serviceResult.Message;
                return Ok(_apiResponse);
            }
        }
        [HttpDelete("member")]
        public async Task<ActionResult<ApiResponse>> KickMember([FromQuery] string memberId)
        {
            await _organizationService.KickMember(memberId);
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        #endregion

        #region ManageUserRole
        [HttpGet("user-roles")]
        public async Task<ActionResult<ApiResponse>> GetUserRoles([FromQuery] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var idRoles = await _db.UserRoles
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToArrayAsync();

            return Ok(new ApiResponse
            {
                Result = idRoles,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        [HttpPost("user-roles")] 
        public async Task<ActionResult<ApiResponse>> AddRoleUser([FromBody] RoleUserCreateDto roleUserCreateDto)
        {
            var user = await _userManager.FindByIdAsync(roleUserCreateDto.IdUser);

            await _db.UserRoles
               .Where(ur => ur.UserId == roleUserCreateDto.IdUser)
               .ExecuteDeleteAsync();

            foreach (var role in roleUserCreateDto.IdRoles)
            {
                await _userManager.AddToRoleByRoleIdAsync(user, role);
            }
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        #endregion

    }
}
