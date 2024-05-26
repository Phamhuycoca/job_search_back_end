using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Job_Seeker_Refresh_Token:BaseEntity
    {
        public Guid Job_SeekerId { get; set; }
        public DateTime Refresh_TokenExpires { get; set; }
        public string? RefreshToken { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public Job_Seeker? Job_Seeker { get; set; }
    }
}
