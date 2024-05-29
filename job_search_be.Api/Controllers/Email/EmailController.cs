using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Dto.Email;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace job_search_be.Api.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IEmployersService _employersService;
        public EmailController(IEmailService emailService, IEmployersService employersService)
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
                send.CompannyName = companny.Data.CompanyName;
                send.Contact = companny.Data.ContactPhoneNumber;
                MailRequest mailrequest = new MailRequest();
                mailrequest.ToEmail = send.MailSend;
                mailrequest.Subject = send.From;
                mailrequest.Body = GetHtmlcontent(send);
                await _emailService.SendEmailAsync(mailrequest);
                return Ok(new { message = "Đã gửi email thành công", statusCode = 200, success = true });
            }

        }
        [NonAction]
        /* private string GetHtmlcontent(SendMail send)
         {
             string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
             Response += "<h1>"+send.Title+"</h1>";
             Response += "<span>" +send.CompannyName+" chúc mừng</span>";
             Response += "<img style='width:100px;height:100px;margin:auto;' src=\"" + send.LogoCompanny + "\" alt=\"Company Logo\" />";
             Response += "<h2>"+send.Content+ ""+send.Date+"</h2>";
             Response += "<a href=\""+send.LinkCompanny+ "\">Please join membership by click the link</a>";
             Response += "<div><h1> Contact us : "+send.Contact+"</h1></div>";
             Response += "</div>";
             return Response;
         }*/
        private string GetHtmlcontent(SendMail send)
        {
            string Response = $@"
    <div style='width: 100%; background-color: #f9f9f9; padding: 20px; font-family: Arial, sans-serif;'>
        <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
            <div style='text-align: center;'>
                <img style='width: 100px; height: 100px; margin-bottom: 20px;' src='{send.LogoCompanny}' alt='Company Logo' />
            </div>
            <h1 style='color: #333333; text-align: center;'>{send.Title}</h1>
            <p style='font-size: 18px; color: #333333; text-align: center;'>{send.CompannyName} chúc mừng bạn!</p>
            <p style='font-size: 16px; color: #555555; text-align: center;'>Chúng tôi vui mừng thông báo rằng bạn đã được chọn để tham gia vào vòng phỏng vấn.</p>
            <div style='text-align: left; margin-top: 20px;'>
                <h2 style='font-size: 20px; color: #333333;'>Chi tiết phỏng vấn:</h2>
                <p style='font-size: 16px; color: #555555;'>
                    <strong>Nội dung:</strong> {send.Content}<br>
                    <strong>Ngày:</strong> {send.Date}<br>
                </p>
                <p style='font-size: 16px; color: #555555;'>Vui lòng tham gia cuộc phỏng vấn bằng cách nhấp vào liên kết bên dưới:</p>
                <div style='text-align: center; margin: 20px 0;'>
                    <a href='{send.LinkCompanny}' style='display: inline-block; padding: 10px 20px; background-color: #007BFF; color: #ffffff; text-decoration: none; border-radius: 5px;'>
                        Tham gia phỏng vấn
                    </a>
                </div>
            </div>
            <div style='border-top: 1px solid #eaeaea; margin-top: 20px; padding-top: 20px; text-align: center;'>
                <h3 style='font-size: 18px; color: #333333;'>Liên hệ với chúng tôi</h3>
                <p style='font-size: 16px; color: #555555;'>{send.Contact}</p>
            </div>
        </div>
    </div>";

            return Response;
        }


    }
}
