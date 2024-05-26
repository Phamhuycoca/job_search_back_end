using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Domain.Dto.FileCv;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.FileCv
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileCvController : ControllerBase
    {
        private readonly IFileCvService _fileCvService;
        public FileCvController(IFileCvService fileCvService)
        {
            _fileCvService = fileCvService;
        }
        [HttpGet]
        public IActionResult Items([FromQuery] CommonListQuery query)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_fileCvService.Items(query, Guid.Parse(objId)));
        }
        [HttpPost]
        public IActionResult PostCV([FromForm] FileCvDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            dto.Job_SeekerId=Guid.Parse(objId);
            return Ok(_fileCvService.Create(dto,url));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_fileCvService.Delete(id));
        }
    }
}
