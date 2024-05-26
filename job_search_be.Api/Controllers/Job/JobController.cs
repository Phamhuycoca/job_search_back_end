using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using job_search_be.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace job_search_be.Api.Controllers.Job
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IEmployersService _employersService;
        public JobController(IJobService jobService, IEmployersService employersService)
        {
            _jobService = jobService;
            _employersService = employersService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_jobService.Items(query,Guid.Parse(objId)));
        }
        //[Authorize]
        [HttpPost]
        public IActionResult Create(JobDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            var checkId = _employersService.ItemsList().Data.Where(x => x.EmployersId == Guid.Parse(objId)).FirstOrDefault();
            if (checkId == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, "Không tìm thấy thông tin nhà tuyển dụng");
            }

            dto.EmployersId = checkId.EmployersId;
            return Ok(_jobService.Create(dto));
        }
        //[Authorize]
        [HttpPatch("{id}")]
        public IActionResult Update(JobDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {

                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            var checkId = _employersService.ItemsList().Data.Where(x => x.EmployersId == Guid.Parse(objId)).FirstOrDefault();
            if (checkId == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, "Không tìm thấy thông tin nhà tuyển dụng");
            }
            return Ok(_jobService.Update(dto));
        }
        //[Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            var checkId = _employersService.ItemsList().Data.Where(x => x.EmployersId == Guid.Parse(objId)).FirstOrDefault();
            if (checkId == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, "Không tìm thấy thông tin nhà tuyển dụng");
            }
            return Ok(_jobService?.Delete(id));
        }
        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            var checkId = _employersService.ItemsList().Data.Where(x => x.EmployersId == Guid.Parse(objId)).FirstOrDefault();
            if (checkId == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, "Không tìm thấy thông tin nhà tuyển dụng");
            }
            return Ok(_jobService.GetById(id));
        }
    }
}
