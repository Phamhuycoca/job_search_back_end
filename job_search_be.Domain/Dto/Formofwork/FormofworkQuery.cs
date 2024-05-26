using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Formofwork
{
    public class FormofworkQuery:BaseEntity
    {
        public Guid FormofworkId { get; set; }
        public string? FormofworkName { get; set; }
    }
}
