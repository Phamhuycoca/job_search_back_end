using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Workexperience
{
    public class WorkexperienceQuery:BaseEntity
    {
        public Guid WorkexperienceId { get; set; }
        public string? WorkexperienceName { get; set; }
    }
}
