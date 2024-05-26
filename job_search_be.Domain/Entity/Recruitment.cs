using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Recruitment:BaseEntity
    {
        public Guid RecruitmentId { get; set; }
        public Guid Job_SeekerId { get; set; }
        public Guid JobId { get; set; }
        public Guid EmployersId { get; set; }
        public string? RecruitmentDateTime { get; set; }
        public string? Content {  get; set; }
        public bool? IsStatus{ get; set; }
        public Job_Seeker? Job_Seeker { get; set; }
        public Job? Job { get; set; }
        public Employers? Employers { get; set; }
        public bool? IsFeedback {  get; set; }
        public Recruitment() 
        {
            IsStatus = false;
            IsFeedback = null;
        }

    }
}
