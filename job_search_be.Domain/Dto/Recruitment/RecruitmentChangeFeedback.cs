using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Recruitment
{
    public class RecruitmentChangeFeedback
    {
        public Guid RecruitmentId { get; set; }

        public bool? IsFeedback { get; set; }
    }
}
