using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Profession;
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
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IMapper _mapper;
        public SalaryService(ISalaryRepository salaryRepository, IMapper mapper)
        {
            _salaryRepository = salaryRepository;
            _mapper = mapper;
        }

        public DataResponse<SalaryQuery> Create(SalaryDto dto)
        {
            dto.SalaryId = Guid.NewGuid();
            var newData = _salaryRepository.Create(_mapper.Map<Salary>(dto));
            if (newData != null)
            {
                return new DataResponse<SalaryQuery>(_mapper.Map<SalaryQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<SalaryQuery> Delete(Guid id)
        {
            var item = _salaryRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _salaryRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<SalaryQuery>(_mapper.Map<SalaryQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<SalaryQuery> GetById(Guid id)
        {
            var item = _salaryRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<SalaryQuery>(_mapper.Map<SalaryQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<SalaryQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<SalaryQuery>>(_salaryRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x => x.SalaryPrice.Contains(commonList.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<SalaryQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<SalaryQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<SalaryDto>> ItemsList()
        {
            var query = _mapper.Map<List<SalaryDto>>(_salaryRepository.GetAllData());
            if (query != null)
            {
                return new DataResponse<List<SalaryDto>>(_mapper.Map< List<SalaryDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<SalaryQuery> Update(SalaryDto dto)
        {
            var item = _salaryRepository.GetById(dto.SalaryId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _salaryRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<SalaryQuery>(_mapper.Map<SalaryQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
