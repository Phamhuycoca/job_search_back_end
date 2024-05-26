using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Job_Seeker
{
    public class Job_SeekerQuery:BaseEntity
    {
        public Guid Job_SeekerId { get; set; }
        public string? FullName { get; set; }
        public string? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Job_Cv { get; set; }
        public string? Academic_Level { get; set; }
        public string? Role { get; set; }
    }
}
