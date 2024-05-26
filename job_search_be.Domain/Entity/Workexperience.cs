using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Workexperience:BaseEntity
    {
        public Guid WorkexperienceId {  get; set; }
        public string? WorkexperienceName { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}
