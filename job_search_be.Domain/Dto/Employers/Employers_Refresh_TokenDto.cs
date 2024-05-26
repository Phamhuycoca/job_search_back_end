using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Employers
{
    public class Employers_Refresh_TokenDto
    {
        public Guid EmployersId { get; set; }
        public DateTime Refresh_TokenExpires { get; set; }
        public string? RefreshToken { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
