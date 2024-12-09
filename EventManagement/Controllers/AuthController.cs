using EventManagement.Common;
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Models;
using EventManagement.Models.ModelsDto;
using EventManagement.Models.ModelsDto.Profile;
using EventManagement.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace EventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBlobService _blobService;
        private readonly SendMailService _sendMailService;

        public AuthController(ApplicationDbContext db, IConfiguration configuration,
                    UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
                    SignInManager<ApplicationUser> signInManager, SendMailService sendMailService,
                    IBlobService blobService)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSetting:Secret");
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _sendMailService = sendMailService;
            _blobService = blobService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = _response.ErrorMessages = ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(ms => $"[{ms.Key}] : {ms.Value.Errors.FirstOrDefault()?.ErrorMessage}")
                    .ToList();
                return BadRequest(_response);
            }

            ApplicationUser userFromDb = _db.ApplicationUsers
                        .FirstOrDefault(u => u.UserName.ToLower() == model.Email.ToLower());

            if(userFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Account does not exist");
                return BadRequest(_response);
            }

            if(userFromDb.LockoutEnabled)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Your account is locked");
                return BadRequest(_response);
            }

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);

            if (isValid == false)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            //Generate JWT Token
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", userFromDb.Email),
                    new Claim("fullName", userFromDb.FullName),
                    new Claim("id", userFromDb.Id.ToString()),
                    new Claim("urlImage", userFromDb.UrlImage??  String.Empty),
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponse = new()
            {
                Email = userFromDb.Email,
                Token = tokenHandler.WriteToken(token),
                FullName = userFromDb.FullName
            };

           
            if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            ApplicationUser userFromDb = await _userManager.FindByEmailAsync(model.Email);

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Account already exists");
                return BadRequest(_response);
            }

            ApplicationUser newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error From Server");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add("Error while registering");
            return BadRequest(_response);
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            ApplicationUser userFromDb = _db.ApplicationUsers
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (userFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("No account found with this email");
                return BadRequest(_response);
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(userFromDb);
            string encodedToken = HttpUtility.UrlEncode(token);

            //var callbackUrl = Url.Action("resetpassword", "auth", new { token, email = userFromDb.Email }, Request.Scheme);
            string callBackUrl = SD.SD_URL_LINK_RESETPASSWORD + $"?token={encodedToken}&email={userFromDb.Email}";

            //await _emailSender.SendEmailAsync(email, "Reset Password",
            //    $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");
            
            await _sendMailService.SendMail(new MailContent { To = email, Body = $"<a href='{callBackUrl}'>clicking here</a>.", Subject="Đổi mật khẩu" });


            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("resetpassword")]
        public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto resetPassordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPassordDto.Email);
            if (user is null)
            {
                return BadRequest("Invalid Request");
            }

            //Day la cho xac thuc token
            var result = await _userManager.ResetPasswordAsync(user, resetPassordDto.Token, resetPassordDto.NewPassword);
            if(!result.Succeeded)
            {
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            _response.IsSuccess =true;
            return Ok(_response);
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
            }
        }

        [HttpGet("profile/{userId}")]
        public async Task<ActionResult<ApiResponse>> GetProfile([FromRoute] string userId)
        {
            try
            {
                ApplicationUser userFromDb = await _userManager.FindByIdAsync(userId);

                if (userFromDb == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("User not found");
                    return NotFound(_response);
                }

                ProfileDto profile = new ProfileDto
                {
                    UrlImage = userFromDb.UrlImage,
                    FullName = userFromDb.FullName,
                    Email = userFromDb.Email
                };

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = profile;
                return Ok(_response);
            }
            catch (Exception)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error retrieving user profile");
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut("profile/{userId}")]
        public async Task<ActionResult<ApiResponse>> PutProfile([FromRoute] string userId, [FromForm] UpdateProfileDto modelUpdateDto)
        {
            var userEntity = await _userManager.FindByIdAsync(userId);

            if (modelUpdateDto.File != null && modelUpdateDto.File.Length > 0 )
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelUpdateDto.File.FileName)}";
                if (string.IsNullOrEmpty(userEntity.UrlImage))
                {
                    userEntity.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelUpdateDto.File);
                }
                else
                {
                    await _blobService.DeleteBlob(userEntity.UrlImage.Split('/').Last(), SD.SD_Storage_Containter);
                    userEntity.UrlImage = await _blobService.UploadBlob(fileName, SD.SD_Storage_Containter, modelUpdateDto.File);
                }
            }
            userEntity.FullName = modelUpdateDto.FullName;
            
            var result = await _userManager.UpdateAsync(userEntity);

            if (result.Succeeded)
            {
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error updating user profile");
                return BadRequest(_response);
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse>> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = users.Select(user => new
            {
                user.UrlImage,
                user.Id,
                user.FullName,
                user.Email,
                user.LockoutEnabled,
            });

            _response.Result = result;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPatch("lockout/user/{userId}")]
        public async Task<ActionResult<ApiResponse>> ChangeLockoutUser([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Result = "Người dùng không tồn tại.";
                return NotFound(_response);
            }

            // Kiểm tra xem có cần khóa người dùng không
            if (user.LockoutEnabled)
            {
                // Nếu đang khóa thì mở khóa
                user.LockoutEnd = null;
            }
            else
            {
                // Nếu chưa khóa thì thiết lập LockoutEnd để khóa tài khoản
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(1); // Khóa trong 1 năm
            }

            // Cập nhật trạng thái LockoutEnabled
            user.LockoutEnabled = !user.LockoutEnabled;

            // Cập nhật thông tin người dùng
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _response.IsSuccess = false;
                _response.Result = "Không thể cập nhật trạng thái khóa người dùng.";
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.Result = user.LockoutEnabled ? "Người dùng đã bị khóa." : "Người dùng đã được mở khóa.";
            return Ok(_response);
        }

    }
}
