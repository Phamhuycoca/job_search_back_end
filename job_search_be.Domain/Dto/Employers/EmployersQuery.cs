using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Employers
{
    public class EmployersQuery:BaseEntity
    {
        public Guid EmployersId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyLogo { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public int? IsStatus { get; set; }
        public Guid? CityId { get; set; }
        public string? Role { get; set; }
        public long? View { get; set; }

    }
}
