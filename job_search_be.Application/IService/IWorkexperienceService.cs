using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Salary;
using job_search_be.Domain.Dto.Workexperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IWorkexperienceService
    {
        PagedDataResponse<WorkexperienceQuery> Items(CommonListQuery commonList);
        DataResponse<List<WorkexperienceDto>> ItemsList();
        DataResponse<WorkexperienceQuery> Create(WorkexperienceDto dto);
        DataResponse<WorkexperienceQuery> Update(WorkexperienceDto dto);
        DataResponse<WorkexperienceQuery> Delete(Guid id);
        DataResponse<WorkexperienceQuery> GetById(Guid id);
    }
}
