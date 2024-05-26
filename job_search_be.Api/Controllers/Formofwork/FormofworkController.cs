using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Formofwork
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormofworkController : ControllerBase
    {
        private readonly IFormofworkService _formofworkService;
        public FormofworkController(IFormofworkService formofworkService)
        {
            _formofworkService = formofworkService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_formofworkService.Items(query));
        }

        [HttpPost]
        public IActionResult Create(FormofworkDto dto)
        {
            return Ok(_formofworkService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(FormofworkDto dto)
        {
            return Ok(_formofworkService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_formofworkService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_formofworkService.GetById(id));
        }
        [HttpGet("ItemsList")]
        public IActionResult ItemsList()
        {
            return Ok(_formofworkService.ItemsList());
        }
    }
}
