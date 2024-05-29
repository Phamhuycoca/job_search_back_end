using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class City:BaseEntity
    {
        public Guid CityId { get; set; }
        public string? CityName { get; set; }     
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<Employers>? Employers { get; set; }

    }
}
