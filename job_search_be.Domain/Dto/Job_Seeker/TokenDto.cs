using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Job_Seeker
{
    public class TokenDto
    {
        public string? AccessToken { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string? RefreshToken { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string? Role { get; set; }

    }
}
