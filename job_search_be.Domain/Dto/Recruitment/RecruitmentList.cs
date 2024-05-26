using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Recruitment
{
    public class RecruitmentList
    {
        public Guid RecruitmentId { get; set; }
        public Guid? Job_SeekerId { get; set; }
        public string? FullName { get; set; }
        public string? CompanyLogo { get; set; }
        public string? Job_Cv { get; set; }
        public Guid? JobId { get; set; }
        public string? Avatar { get; set; }
        public Guid? EmployersId { get; set; }
        public string? JobName { get; set; }
        public string? RecruitmentDateTime { get; set; }
        public bool? IsStatus { get; set; }
        public string? Content { get; set; }
        public bool? IsFeedback { get; set; }
        public Guid? WorkexperienceId { get; set; }
        public Guid? FormofworkId { get; set; }
        public Guid? ProfessionId { get; set; }

    }
}
