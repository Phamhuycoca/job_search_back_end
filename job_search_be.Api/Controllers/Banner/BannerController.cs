using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Banner;
using job_search_be.Domain.Dto.News;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.Banner
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService; 
        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] CommonListQuery query)
        {
            return Ok(_bannerService.Items(query));
        }

        [HttpPost]
        public IActionResult Create([FromForm] BannerDto dto)
        {
           
            return Ok(_bannerService.Create(dto));
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromForm] BannerDto dto)
        {
            return Ok(_bannerService.Update(dto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_bannerService?.Delete(id));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_bannerService.GetById(id));
        }
    }
}
