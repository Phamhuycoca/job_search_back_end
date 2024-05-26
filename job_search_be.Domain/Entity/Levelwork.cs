using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Levelwork:BaseEntity
    {
        public Guid LevelworkId {  get; set; }
        public string? LevelworkName { get; set; }
        public Guid ProfessionId { get; set; }
        public Profession? Profession { get; set; }
        public ICollection<Job>? Jobs { get; set; }

    }
}
