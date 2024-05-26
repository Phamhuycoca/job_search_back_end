using job_search_be.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync( MailRequest mailrequest);
    }
}
