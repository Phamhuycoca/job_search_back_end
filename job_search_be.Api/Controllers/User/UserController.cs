using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Domain.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_userService.Items(query));
        }

        [HttpPost]
        public IActionResult Create([FromForm]UserDto dto)
        {
            return Ok(_userService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromForm] UserDto dto)
        {            
            return Ok(_userService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_userService?.Delete(id));
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_userService.GetById(id));
        }
    }
}
