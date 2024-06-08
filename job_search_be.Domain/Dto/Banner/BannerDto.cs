using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Banner
{
    public class BannerDto
    {
        public Guid BannerId { get; set; }
        public string? BannerPath { get; set; }
        public IFormFile? file { get; set; }
    }
}
