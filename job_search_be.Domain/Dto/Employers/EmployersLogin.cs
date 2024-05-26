using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Employers
{
    public class EmployersLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
