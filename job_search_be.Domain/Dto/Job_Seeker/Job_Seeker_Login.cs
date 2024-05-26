using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Job_Seeker
{
    public class Job_Seeker_Login
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
