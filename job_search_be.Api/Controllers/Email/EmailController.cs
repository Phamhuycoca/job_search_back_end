using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Email;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace job_search_be.Api.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] SendMail send)
        {
            try
            {
                MailRequest mailrequest = new MailRequest();
                mailrequest.ToEmail = "phamkhachuy2472@gmail.com";
                mailrequest.Subject = "Phạm Khắc Huy";
                mailrequest.Body = GetHtmlcontent(send);
                await _emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string GetHtmlcontent(SendMail send)
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>"+send.Title+"</h1>";
            Response += "<img src=\"" + send.LogoCompanny + "\" alt=\"Company Logo\" />";
            Response += "<h2>"+send.Content+"</h2>";
            Response += "<a href=\""+send.LinkCompanny+ "\">Please join membership by click the link</a>";
            Response += "<div><h1> Contact us : "+send.Contact+"</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
