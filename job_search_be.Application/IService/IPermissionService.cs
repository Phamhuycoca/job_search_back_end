using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IPermissionService
    {
        PagedDataResponse<PermissionQuery> Items(CommonListQuery commonListQuery);
        DataResponse<PermissionQuery> Create(PermissionDto dto);
        DataResponse<PermissionQuery> Update(PermissionDto dto);
        DataResponse<PermissionQuery> Delete(string id);
    }
}
