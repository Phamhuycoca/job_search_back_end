using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Job_Seeker
{
    public class UploadCV
    {
        public Guid Job_SeekerId { get; set; }
        public string? Job_Cv { get; set; }
        public IFormFile? cv { get; set; }
        public string? DeleteCv { get; set; }
    }
}
