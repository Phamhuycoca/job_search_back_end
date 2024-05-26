using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Job
{
    public class Job_Recruitment:BaseEntity
    {
        public Guid JobId { get; set; }
        public string? JobName { get; set; }
        public string? JobDescription { get; set; }
        public string? RequestJob { get; set; }
        public string? BenefitsJob { get; set; }
        public string? AddressJob { get; set; }
        public string? WorkingTime { get; set; }
        public string? ExpirationDate { get; set; }
        public Guid? WorkexperienceId { get; set; }
        public string? WorkexperienceName { get; set; }
        public Guid? FormofworkId { get; set; }
        public string? FormofworkName { get; set; }
        public Guid? CityId { get; set; }
        public string? CityName { get; set; }
        public Guid? SalaryId { get; set; }
        public string? SalaryPrice { get; set; }
        public Guid? EmployersId { get; set; }
        public Guid? ProfessionId { get; set; }
        public string? ProfessionName { get; set; }
        public Guid? LevelworkId { get; set; }
        public string? LevelworkName { get; set; }
        public string? totals_cv {  get; set; }
    }
}
