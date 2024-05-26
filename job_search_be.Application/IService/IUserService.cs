using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IUserService
    {
        PagedDataResponse<UserQuery> Items(CommonListQuery commonList);
        DataResponse<UserQuery> Create(UserDto dto);
        DataResponse<UserQuery> Update(UserDto dto);
        DataResponse<UserQuery> Delete(Guid id);
        DataResponse<UserQuery> GetById(Guid id);
        List<Permission> GetUserPermissions(Guid id);
    }
}
