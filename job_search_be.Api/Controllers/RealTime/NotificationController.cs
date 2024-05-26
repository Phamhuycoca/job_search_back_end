using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using job_search_be.Api.Hubs;
using job_search_be.Application.IService;
using job_search_be.Application.Helpers;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using System.Security.Claims;
using job_search_be.Domain.Dto.Notification;

namespace job_search_be.Api.Controllers.RealTime
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;

        public NotificationController(IHubContext<NotificationHub> hubContext,INotificationService notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }
        [HttpGet("GetNotificationByJobSeeker")]
        public async Task<IActionResult> GetNotificationByJobSeeker([FromQuery] CommonListQuery query)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Hello");
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_notificationService.Items(query,Guid.Parse(objId)));
        }
        [HttpGet("GetNotificationByEmployer")]
        public async Task<IActionResult> GetNotificationByEmployer()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Hello");
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            return Ok(_notificationService.ItemsListByEmployer(Guid.Parse(objId)));
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification(NotificationDto dto)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Hello");
            dto.Job_SeekerId= Guid.Parse(objId);
            _notificationService.Create(dto);
            return Ok();
        }
    }
}
