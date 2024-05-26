using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Workexperience;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.Employers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployersService _employersService;
        public EmployersController(IEmployersService employersService)
        {
            _employersService = employersService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_employersService.Items(query));
        }
        [HttpPost]
        public IActionResult Create(EmployersDto dto)
        {
            return Ok(_employersService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromForm]EmployersUpdateDto dto)
        {
            return Ok(_employersService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_employersService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            //_employersService.GetCompannyView(id);
            return Ok(_employersService.GetById(id));
        }
        [HttpGet("ItemsList")]
        public IActionResult ItemsList()
        {
            return Ok(_employersService.ItemsList());
        }
        /*  [HttpPost("Login")]
          public IActionResult Login(EmployersLogin dto)
          {
              return Ok(_employersService.Login(dto));
          }
          [HttpPost("Refresh_token")]
          public IActionResult Refresh_token([FromBody] RefreshTokenSettings refreshToken)
          {
              return Ok(_employersService.Refresh_Token(refreshToken));
          }*/
        [Authorize]
        [HttpGet("GetInfo")]
        public IActionResult GetById()
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_employersService.GetById(Guid.Parse(objId)));
        }
        [HttpGet("GetListJobById/{id}")]
        public IActionResult GetListJobById([FromQuery] CommonListQuery query ,Guid id)
        {
            return Ok(_employersService.GetListJobById(query ,id));
        }
        [HttpGet("CompannyList")]
        public IActionResult CompannyList([FromQuery] CommonQueryByHome query)
        {
            return Ok(_employersService.CompannyList(query));
        }
    }
}
