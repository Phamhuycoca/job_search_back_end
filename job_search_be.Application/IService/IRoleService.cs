using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IRoleService
    {
        PagedDataResponse<RoleQuery> Items(CommonListQuery commonList);
        DataResponse<RoleQuery> Create(RoleDto dto);
        DataResponse<RoleQuery> Update(RoleDto dto);
        DataResponse<RoleQuery> Delete(Guid id);
        DataResponse<RoleQuery> GetById(Guid id);
    }
}
