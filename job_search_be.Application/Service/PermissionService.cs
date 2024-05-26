using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Repositories;
using job_search_be.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public DataResponse<PermissionQuery> Create(PermissionDto dto)
        {
            dto.PermissionId = Guid.NewGuid().ToString();
            var newData = _permissionRepository.Create(_mapper.Map<Permission>(dto));
            if(newData != null)
            {
                return new DataResponse<PermissionQuery>(_mapper.Map<PermissionQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<PermissionQuery> Delete(string id)
        {
            var item = _permissionRepository.GetByString(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data=_permissionRepository.DeleteByString(id);
            if(data != null)
            {
                return new DataResponse<PermissionQuery>(_mapper.Map<PermissionQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);

        }

        public PagedDataResponse<PermissionQuery> Items(CommonListQuery commonListQuery)
        {
            var query = _mapper.Map<List<PermissionQuery>>(_permissionRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonListQuery.keyword))
            {
                query = query.Where(x => x.PermissionName.Contains(commonListQuery.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<PermissionQuery>.ToPageList(query, commonListQuery.page, commonListQuery.limit);
            return new PagedDataResponse<PermissionQuery>(paginatedResult, 200,query.Count());

        }

        public DataResponse<PermissionQuery> Update(PermissionDto dto)
        {
            var item = _permissionRepository.GetByString(dto.PermissionId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _permissionRepository.Update(_mapper.Map(dto, item));
            if(newData != null)
            {
                return new DataResponse<PermissionQuery>(_mapper.Map<PermissionQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);

        }
    }
}
