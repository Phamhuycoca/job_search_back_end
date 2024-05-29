using job_search_be.Domain.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.News
{
    public class NewsDto
    {
        public Guid NewsId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? PathImage { get; set; }
        public DateTime? NewsCreate { get; set; }
        public Guid UserId { get; set; }
        public IFormFile? file { get; set; }
    }
}
