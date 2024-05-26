using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IFormofworkService
    {
        PagedDataResponse<FormofworkQuery> Items(CommonListQuery commonList);
        DataResponse<List<FormofworkDto>> ItemsList();
        DataResponse<FormofworkQuery> Create(FormofworkDto dto);
        DataResponse<FormofworkQuery> Update(FormofworkDto dto);
        DataResponse<FormofworkQuery> Delete(Guid id);
        DataResponse<FormofworkQuery> GetById(Guid id);
    }
}
