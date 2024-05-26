using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Levelwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Levelwork
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelworkController : ControllerBase
    {
        private readonly ILevelworkService _levelworkService;
        public LevelworkController(ILevelworkService levelworkService)
        {
            _levelworkService = levelworkService;
        }
             
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_levelworkService.Items(query));
        }

        [HttpPost]
        public IActionResult Create(LevelworkDto dto)
        {
            return Ok(_levelworkService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(LevelworkDto dto)
        {
            return Ok(_levelworkService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_levelworkService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_levelworkService.GetById(id));
        }
        [HttpGet("ItemsList")]
        public IActionResult ItemsList()
        {
            return Ok(_levelworkService.ItemsList());
        }
        [HttpGet("GetByProfession/{id}")]
        public IActionResult GetByProfession(Guid id)
        {
            return Ok(_levelworkService.GetByProfession(id));
        }
    }
}
