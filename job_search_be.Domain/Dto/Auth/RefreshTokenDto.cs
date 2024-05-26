using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Auth
{
    public class RefreshTokenDto
    {
        public Guid UserId { get; set; }
        public DateTime Refresh_TokenExpires { get; set; }
        public string? RefreshToken { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
