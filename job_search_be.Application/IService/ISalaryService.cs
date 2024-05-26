using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface ISalaryService
    {
        PagedDataResponse<SalaryQuery> Items(CommonListQuery commonList);
        DataResponse<List<SalaryDto>> ItemsList();
        DataResponse<SalaryQuery> Create(SalaryDto dto);
        DataResponse<SalaryQuery> Update(SalaryDto dto);
        DataResponse<SalaryQuery> Delete(Guid id);
        DataResponse<SalaryQuery> GetById(Guid id);
    }
}
