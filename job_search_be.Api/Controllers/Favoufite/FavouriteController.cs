using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Favourite;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.Favoufite
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteJobService _favouriteJobService;
        public FavouriteController(IFavouriteJobService favouriteJobService)
        {
            _favouriteJobService = favouriteJobService;
        }
        [HttpGet("Favourite_Jobs")]
        //public IActionResult Favourite_Jobs([FromQuery] CommonListQuery commonListQuery)
        public IActionResult Favourite_Jobs()
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            //return Ok(_favouriteJobService.Favourite_Jobs(commonListQuery, Guid.Parse(objId)));
            return Ok(_favouriteJobService.Favourite_Jobs(Guid.Parse(objId)));
        }
        [HttpPost("FavouriteJob")]
        public IActionResult FavouriteJob(FavouriteJob dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            var favourite = new FavouriteJobDto();
            if (dto.Favoufite_Job_Id == "")
            {
                favourite.Favoufite_Job_Id = new Guid();
            }
            else
            {
                favourite.Favoufite_Job_Id = Guid.Parse(dto.Favoufite_Job_Id);
            }
            favourite.IsFavoufite_Job = Boolean.Parse(dto.IsFavoufite_Job);
            favourite.JobId = Guid.Parse(dto.JobId);
            favourite.Job_SeekerId = Guid.Parse(objId);
            return Ok(_favouriteJobService.Favourite(favourite));
            //return Ok(dto);
        }
    }
}
