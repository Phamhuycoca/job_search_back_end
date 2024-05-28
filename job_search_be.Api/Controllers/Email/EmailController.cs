using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Email;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IEmployersService _employersService;
        public EmailController(IEmailService emailService,IEmployersService employersService)
        {
            _emailService = emailService;
            _employersService = employersService;
        }
        //[Authorize]
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] SendMail send)
        {
            var objId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (objId == null)
            {
                throw new ApiException(HttpStatusCode.FORBIDDEN, HttpStatusMessages.Forbidden);
            }
            else
            {
                var companny = _employersService.GetById(Guid.Parse(objId));
                send.LogoCompanny = companny.Data.CompanyLogo;
                send.LinkCompanny = companny.Data.CompanyWebsite;
                send.CompannyName= companny.Data.CompanyName;
                send.Contact = companny.Data.ContactPhoneNumber;
                MailRequest mailrequest = new MailRequest();
                mailrequest.ToEmail = send.MailSend;
                mailrequest.Subject =send.From;
                mailrequest.Body = GetHtmlcontent(send);
                await _emailService.SendEmailAsync(mailrequest);
                return Ok(new {message="Đã gửi email thành công", statusCode = 200, success =true});
            }
           
        }
        [NonAction]
        private string GetHtmlcontent(SendMail send)
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>"+send.Title+"</h1>";
            Response += "<span>" +send.CompannyName+" chúc mừng</span>";
            Response += "<img src=\"" + send.LogoCompanny + "\" alt=\"Company Logo\" />";
            Response += "<h2>"+send.Content+ ""+send.Date+"</h2>";
            Response += "<a href=\""+send.LinkCompanny+ "\">Please join membership by click the link</a>";
            Response += "<div><h1> Contact us : "+send.Contact+"</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
