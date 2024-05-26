using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Profession
{
    public class ProfessionQuery:BaseEntity
    {
        public Guid ProfessionId { get; set; }
        public string? ProfessionName { get; set; }
    }
}
