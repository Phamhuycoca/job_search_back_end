using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.FileCv
{
    public class FileCvDto
    {
        public Guid FileCvId { get; set; }
        public string? HostPath { get; set; }
        public string? FileCVName { get; set; }
        public string? FileCvPath { get; set; }
        public Guid? Job_SeekerId { get; set; }
        public IFormFile? pdfFile { get; set; }
    }
}
