using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Employers : BaseEntity
    {
        public Guid EmployersId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhoneNumber { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyLogo { get; set; }
        public long? View { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }
        public int? IsStatus { get; set; }
        public Guid? CityId { get; set; }
        public string? Role { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public City? City { get; set; }
        public virtual ICollection<Employers_Refresh_Token>? Refresh_Tokens { get; set; }
        public virtual ICollection<Recruitment>? Recruitments { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public Employers()
        {
            Role= "Employer";
            View = 0;
        }

}
}
