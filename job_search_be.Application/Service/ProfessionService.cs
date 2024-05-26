using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Profession;
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
    public class ProfessionService : IProfessionService
    {
        private readonly IProfessionRepository _professionRepository;
        private readonly IMapper _mapper;
        public ProfessionService(IProfessionRepository professionRepository, IMapper mapper)
        {
            _professionRepository = professionRepository;
            _mapper = mapper;
        }

        public DataResponse<ProfessionQuery> Create(ProfessionDto dto)
        {
            dto.ProfessionId = Guid.NewGuid();
            var newData = _professionRepository.Create(_mapper.Map<Profession>(dto));
            if (newData != null)
            {
                return new DataResponse<ProfessionQuery>(_mapper.Map<ProfessionQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<ProfessionQuery> Delete(Guid id)
        {
            var item = _professionRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _professionRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<ProfessionQuery>(_mapper.Map<ProfessionQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<ProfessionQuery> GetById(Guid id)
        {
            var item = _professionRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<ProfessionQuery>(_mapper.Map<ProfessionQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<ProfessionQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<ProfessionQuery>>(_professionRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x => x.ProfessionName.Contains(commonList.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<ProfessionQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<ProfessionQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<ProfessionDto>> ItemsList()
        {
            var query = _mapper.Map<List<ProfessionDto>>(_professionRepository.GetAllData());
            if (query != null)
            {
                return new DataResponse<List<ProfessionDto>>(_mapper.Map< List<ProfessionDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<ProfessionQuery> Update(ProfessionDto dto)
        {
            var item = _professionRepository.GetById(dto.ProfessionId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _professionRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<ProfessionQuery>(_mapper.Map<ProfessionQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
