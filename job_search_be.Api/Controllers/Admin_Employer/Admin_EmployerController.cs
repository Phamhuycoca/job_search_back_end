using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Admin_Employer
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admin_EmployerController : ControllerBase
    {
        private readonly IEmployersRepository _employersRepository;
        private readonly IEmployersService _employersService;
        public Admin_EmployerController(IEmployersRepository employersRepository,IEmployersService employersService)
        {
            _employersRepository = employersRepository;
            _employersService = employersService;
        }
        [HttpGet]
        public IActionResult Index([FromQuery] CommonListQuery commonListQuery)
        {
            return Ok(_employersService.Admin_Employers(commonListQuery));
        }
        [HttpPatch("{id}")]
        public IActionResult Update(Guid id)
        {
            var dto=_employersRepository.GetById(id);
            dto.deletedAt = null;
            return Ok(_employersRepository.Update(dto));
        }
    }
}
