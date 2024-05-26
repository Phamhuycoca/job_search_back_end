using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Helpers
{
    public class CommonQueryByHome
    {
        public int page { get; set; }
        public int limit { get; set; }
        public string cityId { get; set; }
        public string salaryId { get; set; }
        public string professionId { get; set; }
        public string workexperienceId { get; set; }
        public string formofworkId { get; set; }
        public string keyword { get; set; }
        public string? levelworkId {  get; set; }
        public CommonQueryByHome() 
        {
            page = 1;
            limit = 10;
            cityId = "";
            salaryId = "";
            professionId = "";
            workexperienceId = "";
            formofworkId = "";
            keyword = "";
            levelworkId = "";
        }
    }
}
