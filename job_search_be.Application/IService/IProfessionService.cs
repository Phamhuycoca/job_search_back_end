using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Profession;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.Workexperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IProfessionService
    {
        PagedDataResponse<ProfessionQuery> Items(CommonListQuery commonList);
        DataResponse<List<ProfessionDto>> ItemsList();
        DataResponse<ProfessionQuery> Create(ProfessionDto dto);
        DataResponse<ProfessionQuery> Update(ProfessionDto dto);
        DataResponse<ProfessionQuery> Delete(Guid id);
        DataResponse<ProfessionQuery> GetById(Guid id);
    }
}
