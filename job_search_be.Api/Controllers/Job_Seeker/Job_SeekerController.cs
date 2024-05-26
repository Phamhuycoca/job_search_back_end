using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.Job_Seeker
{
    [Route("api/[controller]")]
    [ApiController]
    public class Job_SeekerController : ControllerBase
    {
        private readonly IJob_SeekerService _job_SeekerService;
        public Job_SeekerController(IJob_SeekerService job_SeekerService)
        {
            _job_SeekerService = job_SeekerService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_job_SeekerService.Items(query));
        }
        [HttpPost]
        public IActionResult Create(Job_SeekerDto dto)
        {
            return Ok(_job_SeekerService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromForm] Job_SeekerUpdateDto dto)
        {
            var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return Ok(_job_SeekerService.Update(dto, url)) ;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_job_SeekerService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_job_SeekerService.GetById(id));
        }
        [Authorize]
        [HttpGet("GetInfo")]
        public IActionResult GetById()
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_job_SeekerService.GetById(Guid.Parse(objId)));
        }
        [Authorize]
        [HttpPost("upload-cv")]
        public IActionResult UploadCV([FromForm] UploadCV cv)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            cv.Job_SeekerId = Guid.Parse(objId);
            var url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            return Ok(_job_SeekerService.UploadCv(cv, url));
        }
    }
}
