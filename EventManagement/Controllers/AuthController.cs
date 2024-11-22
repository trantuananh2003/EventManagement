using EventManagement.Common;
using EventManagement.Data.DataConnect;
using EventManagement.Data.Models;
using EventManagement.Models;
using EventManagement.Models.ModelsDto;
using EventManagement.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly SendMailService _sendMailService;

        public AuthController(ApplicationDbContext db, IConfiguration configuration,
                    UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
                    SignInManager<ApplicationUser> signInManager, SendMailService sendMailService)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSetting:Secret");
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _sendMailService = sendMailService;
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
            ApplicationUser userFromDb = _db.ApplicationUsers
                .FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            if (userFromDb != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Account already exists");
                return BadRequest(_response);
            }

            ApplicationUser newUser = new()
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
                    //Check role
                    if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
                    {
                        //create roles in database
                        //await _roleManager.CreateAsync(new ApplicationRolenew);
                    }

                    //if (model.Role.ToLower() == SD.Role_Customer)
                    //{
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    //}

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
    }
}
