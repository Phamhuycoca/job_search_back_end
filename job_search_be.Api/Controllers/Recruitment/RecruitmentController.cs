using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Recruitment;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace job_search_be.Api.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecruitmentController : ControllerBase
    {
        private readonly IRecruitmentService _recruitmentService;
        public RecruitmentController(IRecruitmentService recruitmentService)
        {
            _recruitmentService = recruitmentService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_recruitmentService.Items(query,Guid.Parse(objId)));
        }
        [HttpPost]
        public IActionResult Create(RecruitmentDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            dto.Job_SeekerId = Guid.Parse(objId);
            return Ok(_recruitmentService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update(RecruitmentDto dto)
        {
            return Ok(_recruitmentService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_recruitmentService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_recruitmentService.GetById(id));
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("ItemsByEmployer/{id}")]
        public IActionResult ItemsByEmployer([FromQuery] CommonQueryByHome query,Guid id)
        {
           /* var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }*/
            return Ok(_recruitmentService.ItemsByEmployer(query, id));
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("ItemsByEmployerId")]
        public IActionResult ItemsByEmployerId()
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_recruitmentService.ItemsByEmployerId(Guid.Parse(objId)));
        }
        [Authorize(Roles = "Employer")]
        [HttpGet("ItemsByEmployerSuitable")]
        public IActionResult ItemsByEmployerSuitable([FromQuery] CommonQueryByHome query)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_recruitmentService.ItemsByEmployerSuitable(query, Guid.Parse(objId)));
        }
        [Authorize(Roles = "Job_seeker")]
        [HttpGet("ItemsByJob_seeker")]
        public IActionResult ItemsByJob_seeker([FromQuery] CommonListQuery query)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;       
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_recruitmentService.ItemsByJob_seeker(query, Guid.Parse(objId)));
        }
        [HttpPatch("ChangeFeedback")]
        public IActionResult ChangeFeedback(RecruitmentChangeFeedback changeFeedback)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_recruitmentService.ChangeFeedback(changeFeedback));
        }
        [HttpPatch("ChangeStatus")]
        public IActionResult ChangeStatus(RecruitmentChangeStatus changeStatus)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_recruitmentService.ChangeStatus(changeStatus));
        }
        [HttpGet("Job_SeekerById/{id}")]
        public IActionResult Job_SeekerById([FromQuery] CommonQueryByHome query, Guid id)
        {
            return Ok(_recruitmentService.Job_SeekerById(query,id));
        }
    }
}
