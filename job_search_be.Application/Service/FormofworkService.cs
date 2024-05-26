using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.Salary;
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
    public class FormofworkService:IFormofworkService
    {
        private readonly IMapper _mapper;
        private readonly IFormofworkRepository _formofworkRepository;
        public FormofworkService(IMapper mapper, IFormofworkRepository formofworkRepository)
        {
            _mapper = mapper;
            _formofworkRepository = formofworkRepository;
        }

        public DataResponse<FormofworkQuery> Create(FormofworkDto dto)
        {
            dto.FormofworkId = Guid.NewGuid();
            var newData = _formofworkRepository.Create(_mapper.Map<Formofwork>(dto));
            if (newData != null)
            {
                return new DataResponse<FormofworkQuery>(_mapper.Map<FormofworkQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<FormofworkQuery> Delete(Guid id)
        {
            var item = _formofworkRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _formofworkRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<FormofworkQuery>(_mapper.Map<FormofworkQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<FormofworkQuery> GetById(Guid id)
        {
            var item = _formofworkRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<FormofworkQuery>(_mapper.Map<FormofworkQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<FormofworkQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<FormofworkQuery>>(_formofworkRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x => x.FormofworkName.Contains(commonList.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<FormofworkQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<FormofworkQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<FormofworkDto>> ItemsList()
        {
            var query = _mapper.Map<List<FormofworkDto>>(_formofworkRepository.GetAllData());
            if (query != null)
            {
                return new DataResponse<List<FormofworkDto>>(_mapper.Map< List<FormofworkDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<FormofworkQuery> Update(FormofworkDto dto)
        {
            var item = _formofworkRepository.GetById(dto.FormofworkId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _formofworkRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<FormofworkQuery>(_mapper.Map<FormofworkQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
