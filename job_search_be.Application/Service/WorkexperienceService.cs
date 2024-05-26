using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Salary;
using job_search_be.Domain.Dto.Workexperience;
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
    public class WorkexperienceService:IWorkexperienceService
    {
        private readonly IWorkexperienceRepository _workexperienceRepository;
        private readonly IMapper _mapper;
        public WorkexperienceService(IWorkexperienceRepository workexperienceRepository, IMapper mapper)
        {
            _workexperienceRepository = workexperienceRepository;
            _mapper = mapper;
        }

        public DataResponse<WorkexperienceQuery> Create(WorkexperienceDto dto)
        {
            dto.WorkexperienceId = Guid.NewGuid();
            var newData = _workexperienceRepository.Create(_mapper.Map<Workexperience>(dto));
            if (newData != null)
            {
                return new DataResponse<WorkexperienceQuery>(_mapper.Map<WorkexperienceQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<WorkexperienceQuery> Delete(Guid id)
        {
            var item = _workexperienceRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _workexperienceRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<WorkexperienceQuery>(_mapper.Map<WorkexperienceQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<WorkexperienceQuery> GetById(Guid id)
        {
            var item = _workexperienceRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<WorkexperienceQuery>(_mapper.Map<WorkexperienceQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<WorkexperienceQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<WorkexperienceQuery>>(_workexperienceRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x => x.WorkexperienceName.Contains(commonList.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<WorkexperienceQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<WorkexperienceQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<WorkexperienceDto>> ItemsList()
        {
            var query = _mapper.Map<List<WorkexperienceDto>>(_workexperienceRepository.GetAllData());
            if (query != null)
            {
                return new DataResponse<List<WorkexperienceDto>>(_mapper.Map< List<WorkexperienceDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<WorkexperienceQuery> Update(WorkexperienceDto dto)
        {
            var item = _workexperienceRepository.GetById(dto.WorkexperienceId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _workexperienceRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<WorkexperienceQuery>(_mapper.Map<WorkexperienceQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
