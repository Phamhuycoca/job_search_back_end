using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Levelwork;
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
    public class LevelworkService : ILevelworkService
    {
        private readonly ILevelworkRepository _levelworkRepository;
        private readonly IMapper _mapper;
        public LevelworkService(ILevelworkRepository levelworkRepository, IMapper mapper)
        {
            _levelworkRepository = levelworkRepository;
            _mapper = mapper;
        }

        public DataResponse<LevelworkQuery> Create(LevelworkDto dto)
        {
            dto.LevelworkId = Guid.NewGuid();
            var newData = _levelworkRepository.Create(_mapper.Map<Levelwork>(dto));
            if (newData != null)
            {
                return new DataResponse<LevelworkQuery>(_mapper.Map<LevelworkQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<LevelworkQuery> Delete(Guid id)
        {
            var item = _levelworkRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _levelworkRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<LevelworkQuery>(_mapper.Map<LevelworkQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<LevelworkQuery> GetById(Guid id)
        {
            var item = _levelworkRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<LevelworkQuery>(_mapper.Map<LevelworkQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public DataResponse<List<LevelworkDto>> GetByProfession(Guid id)
        {
            var query = _mapper.Map<List<Levelwork>>(_levelworkRepository.GetAllData().Where(x=>x.ProfessionId==id).ToList());
            return new DataResponse<List<LevelworkDto>>(_mapper.Map<List<LevelworkDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<LevelworkQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<LevelworkQuery>>(_levelworkRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x => x.LevelworkName.Contains(commonList.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<LevelworkQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<LevelworkQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<LevelworkDto>> ItemsList()
        {
            var query = _levelworkRepository.GetAllData();
            if (query != null && query.Any())
            {
                var cityDtos = _mapper.Map<List<LevelworkDto>>(query);
                return new DataResponse<List<LevelworkDto>>(_mapper.Map<List<LevelworkDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<LevelworkQuery> Update(LevelworkDto dto)
        {
            var item = _levelworkRepository.GetById(dto.LevelworkId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _levelworkRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<LevelworkQuery>(_mapper.Map<LevelworkQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
