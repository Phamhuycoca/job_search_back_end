using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Levelwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface ILevelworkService
    {
        PagedDataResponse<LevelworkQuery> Items(CommonListQuery commonList);
        DataResponse<List<LevelworkDto>> ItemsList();
        DataResponse<List<LevelworkDto>> GetByProfession(Guid id);
        DataResponse<LevelworkQuery> Create(LevelworkDto dto);
        DataResponse<LevelworkQuery> Update(LevelworkDto dto);
        DataResponse<LevelworkQuery> Delete(Guid id);
        DataResponse<LevelworkQuery> GetById(Guid id);
    }
}
