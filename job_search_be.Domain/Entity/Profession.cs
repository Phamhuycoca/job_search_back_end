using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Profession:BaseEntity
    {
        public Guid ProfessionId { get; set; }
        public string? ProfessionName { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<Levelwork>? Levelworks { get; set;}
    }
}
