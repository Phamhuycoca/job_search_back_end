using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Domain.Dto.Permission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Permissions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_permissionService.Items(query));
        }


        [HttpPost]
        public IActionResult Create(PermissionDto dto)
        {
            return Ok(_permissionService.Create(dto));
        }

        [HttpPut("{id}")]
        public IActionResult Update(PermissionDto dto, string id)
        {
            return Ok(_permissionService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok(_permissionService?.Delete(id));
        }
    }
}
