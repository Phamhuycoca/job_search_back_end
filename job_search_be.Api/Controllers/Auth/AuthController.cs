using Azure.Core;
using job_search_be.Application.IService;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace job_search_be.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmployersService _employersService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IEmployers_Refresh_TokenRepository _employers_Refresh_TokenRepository;
        private readonly IJob_SeekerService _jobService;
        private readonly IJob_Seeker_Refresh_Token_Repository _job_seeker_Refresh_TokensRepository;
        public AuthController(IAuthService authService, IEmployersService employersService, IEmployers_Refresh_TokenRepository employers_Refresh_TokenRepository,IRefreshTokenRepository refreshTokenRepository, IJob_SeekerService jobService, IJob_Seeker_Refresh_Token_Repository job_seeker_Refresh_TokensRepository)
        {
            _authService = authService;
            _employersService = employersService;
            _employers_Refresh_TokenRepository = employers_Refresh_TokenRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jobService = jobService;
            _job_seeker_Refresh_TokensRepository = job_seeker_Refresh_TokensRepository;
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginDto dto)
        {
            return Ok(_authService.Login(dto));
        }
        [HttpPost("Refresh_token")]
        public IActionResult Refresh_token([FromBody] RefreshTokenSettings refreshToken)
        {
            var check1 = _refreshTokenRepository.GetAllData().Where(x => x.RefreshToken == refreshToken.Refresh_Token).FirstOrDefault();
            if (check1!=null)
            {
                return Ok(_authService.Refresh_Token(refreshToken));
            }
            var check2=_employers_Refresh_TokenRepository.GetAllData().Where(x=>x.RefreshToken == refreshToken.Refresh_Token).FirstOrDefault();
            if (check2!=null)
            {
                return Ok(_employersService.Refresh_Token(refreshToken));
            }
            var check3=_job_seeker_Refresh_TokensRepository.GetAllData().Where(x=>x.RefreshToken==refreshToken.Refresh_Token).FirstOrDefault();
            if (check3!=null)
            {
                return Ok(_jobService.Refresh_Token(refreshToken));
            }
            throw new ApiException(HttpStatusCode.NOT_FOUND, HttpStatusMessages.NotFound);
        }
        [Authorize]
        [HttpGet("Hello")]
        public IActionResult Get()
        {
            return Ok("Ok");
        }
        [HttpPost("EmployersLogin")]
        public IActionResult Employers_Login(EmployersLogin dto)
        {
            return Ok(_employersService.Login(dto));
        }
        [HttpPost("JobSeekerLogin")]
        public IActionResult JobSeeker_Login(Job_Seeker_Login dto)
        {
            return Ok(_jobService.Login(dto));
        }
        /* [HttpPost("Employers_Refresh_token")]
         public IActionResult Employers_Refresh_token([FromBody] RefreshTokenSettings refreshToken)
         {
             return Ok(_employersService.Refresh_Token(refreshToken));
         }*/
        [HttpPost("google-login")]
        public IActionResult GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            return Ok(_jobService.Job_SeekerLoginByGoole(request));
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Request.Headers.Clear();
            HttpContext.Items.Clear();
            HttpContext.User = null;
            return Ok(new { success = true, data = "", statusCode = 200, message="Đăng xuất thành công" });
        }
        [HttpGet("GetProfileJob_Seeker")]
        public IActionResult GetProfileJob_Seeker()
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_jobService.GetById(Guid.Parse(objId)));
        }
        [HttpPost("JobSeekerRegister")]
        public IActionResult JobSeekerRegister(Job_Seeker_Register dto)
        {
            return Ok(_jobService.Register(dto));
        }
        [HttpPost("EmployerRegister")]
        public IActionResult EmployerRegister(EmployerRegister dto)
        {
            return Ok(_employersService.Register(dto));
        }
    }
}
