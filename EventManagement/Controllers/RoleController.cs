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
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("role/{roleId}")] //Lấy chi tiết về role đó
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

        [HttpPost("role")] //Thêm role vào
        public async Task<ActionResult<ApiResponse>> AddRoleByOrganization([FromBody] RoleCreateDto roleCreateDto)
        {
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

            //Thêm claim vào role
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

        [HttpPut("role/{roleId}")] //Cập nhập role
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

        [HttpDelete("role/{roleId}")] //Xóa role
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

        [HttpGet("o-permissions")] //Lấy toàn bộ quyền hạn của một tổ chức có thể làm
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
        [Authorize(Policy = SD_Role_Permission.ManageMember_ClaimValue)]
        public async Task<ActionResult<ApiResponse>> GetMembers([FromQuery] string idOrganization, string searchString,
            int pageSize = 0, int pageNumber = 1)
        {
            var pagedListDto = await _organizationService.GetAllMemberByIdOrganization(idOrganization,searchString ,pageSize,pageNumber);

            PaginationDto pagination = new PaginationDto()
            {
                CurrentPage = pagedListDto.CurrentPage,
                PageSize = pagedListDto.PageSize,
                TotalRecords = pagedListDto.TotalCount
            };
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));

            return Ok(new ApiResponse
            {
                Result = pagedListDto.Items,
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
        //Lay toàn bộ role của user
        [HttpGet("user-roles")] 
        [Authorize(Policy = SD_Role_Permission.ManageRole_ClaimValue)]
        public async Task<ActionResult<ApiResponse>> GetUserRoles([FromQuery] string userId,
            [FromQuery] string organizationId)
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

        [HttpPost("user-roles")]  //Thêm role cho user
        public async Task<ActionResult<ApiResponse>> UpdateRoleUser([FromBody] RoleUserCreateDto roleUserCreateDto)
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
