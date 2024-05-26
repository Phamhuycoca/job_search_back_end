using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Salary
{
    public class SalaryDto
    {
        public Guid SalaryId { get; set; }
        public string? SalaryPrice { get; set; }
    }
}
