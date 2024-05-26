using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Job:BaseEntity
    {
        public Guid JobId { get; set; }
        public string? JobName { get; set; }
        public string? JobDescription { get; set; }
        public string? RequestJob {  get; set; }
        public string? BenefitsJob { get; set; }
        public string? AddressJob { get; set; }
        public string? WorkingTime {  get; set; }
        public string? ExpirationDate { get; set; }
        public Guid? WorkexperienceId { get; set; }
        public Guid? FormofworkId { get; set; }
        public Guid? CityId {  get; set; }
        public Guid? SalaryId { get; set; }
        public Guid? EmployersId { get; set; }
        public Guid? ProfessionId { get; set; }
        public Guid? LevelworkId { get; set; }
        public Workexperience? Workexperience { get; set; }
        public Formofwork? Formofwork { get; set; }
        public City? City { get; set; }
        public Salary? Salary { get; set; }
        public Employers? Employers { get; set; }
        public Profession? Profession { get; set; }
        public Levelwork? Levelwork { get; set; }
        public virtual ICollection<Recruitment>? Recruitments { get; set; }
        public virtual ICollection<Favoufite_Job>? Favoufite_Jobs { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }

    }
}
