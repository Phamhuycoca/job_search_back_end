using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Dto.Role;
using job_search_be.Infrastructure.Common.Utilities;
using job_search_be.Infrastructure.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Role
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HasPermission(Permission.Modify)]

        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_roleService.Items(query));
        }

        [HttpPost]
        public IActionResult Create(RoleDto dto)
        {
            return Ok(_roleService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(RoleDto dto)
        {
            return Ok(_roleService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_roleService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_roleService.GetById(id));
        }
    }
}
