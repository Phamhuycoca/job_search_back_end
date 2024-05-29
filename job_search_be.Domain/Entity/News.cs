using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class News : BaseEntity
    {
        public Guid NewsId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? PathImage { get; set; }
        public DateTime? NewsCreate {  get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
