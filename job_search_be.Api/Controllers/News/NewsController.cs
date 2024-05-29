using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.News;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.News
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_newsService.Items(query));
        }

        [HttpPost]
        public IActionResult Create([FromForm]NewsDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            dto.UserId = Guid.Parse(objId);
            return Ok(_newsService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromForm] NewsDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            dto.UserId = Guid.Parse(objId);
            return Ok(_newsService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_newsService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_newsService.GetById(id));
        }
    }
}
