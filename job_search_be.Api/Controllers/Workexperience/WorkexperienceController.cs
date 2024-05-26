using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.Workexperience;
using job_search_be.Infrastructure.Common.Utilities;
using job_search_be.Infrastructure.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Workexperience
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkexperienceController : ControllerBase
    {
        private IWorkexperienceService _workexperienceService;
        public WorkexperienceController(IWorkexperienceService workexperienceService)
        {
            _workexperienceService = workexperienceService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_workexperienceService.Items(query));
        }
        [HttpPost]
        public IActionResult Create(WorkexperienceDto dto)
        {
            return Ok(_workexperienceService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(WorkexperienceDto dto)
        {
            return Ok(_workexperienceService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_workexperienceService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_workexperienceService.GetById(id));
        }
        [HttpGet("ItemsList")]
        public IActionResult ItemsList()
        {
            return Ok(_workexperienceService.ItemsList());
        }
    }
}
