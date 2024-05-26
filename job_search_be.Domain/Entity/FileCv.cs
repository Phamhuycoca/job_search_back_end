using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class FileCv:BaseEntity
    {
        public Guid FileCvId {  get; set; }
        public string? FileCVName { get; set; }
        public string? HostPath {  get; set; }
        public string? FileCvPath { get; set; }
        public Guid? Job_SeekerId { get; set; }
        public Job_Seeker? Job_Seeker { get; set; }
    }
}
