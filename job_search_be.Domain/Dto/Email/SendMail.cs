using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Email
{
    public class SendMail
    {
        public string? MailSend { get; set; }
        public string? From { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Contact { get; set; }
        public string? LogoCompanny { get; set; }
        public string? LinkCompanny { get; set; }
        public string? CompannyName { get; set; }
        public string? Date { get; set; }

    }
}
