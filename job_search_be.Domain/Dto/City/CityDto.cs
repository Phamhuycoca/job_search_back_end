using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.City
{
    public class CityDto
    {
        public Guid CityId { get; set; }
        public string? CityName { get; set; }
    }
}
