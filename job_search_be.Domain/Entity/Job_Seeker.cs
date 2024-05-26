using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Job_Seeker : BaseEntity
    {
        public Guid Job_SeekerId { get; set; }
        public string? FullName { get; set; }
        public string? Birthday { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public string? Job_Cv { get; set; }
        public string? Academic_Level {get;set;}
        public string? Role { get; set; }
        public virtual ICollection<Job_Seeker_Refresh_Token>? Refresh_Tokens { get; set; }
        public virtual ICollection<Recruitment>? Recruitments { get; set; }
        public virtual ICollection<FileCv>? FileCvs { get; set; }
        public virtual ICollection<Favoufite_Job>? Favoufite_Jobs { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }

        public Job_Seeker()
        {
            Role = "Job_seeker";
            Avatar = "";
            Job_Cv = "";
        }
    }
}
