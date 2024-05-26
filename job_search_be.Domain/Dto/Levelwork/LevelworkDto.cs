using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Levelwork
{
    public class LevelworkDto
    {
        public Guid LevelworkId { get; set; }
        public string? LevelworkName { get; set; }
        public Guid ProfessionId { get; set; }
    }
}
