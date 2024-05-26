using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.BaseModel
{
    public class BaseEntity
    {
        public Guid? createdBy { get; set; }
        public DateTime? createdAt { get; set; }
        public Guid? updatedBy { get; set; }
        public DateTime? updatedAt { get; set; }
        public Guid? deletedBy { get; set; }
        public DateTime? deletedAt { get; set; }

    }
}
