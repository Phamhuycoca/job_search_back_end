using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.FileCv;
using job_search_be.Domain.Dto.Job_Seeker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IFileCvService
    {
        PagedDataResponse<FileCvQuery> Items(CommonListQuery commonListQuery, Guid id);
        DataResponse<FileCvQuery> Create(FileCvDto dto, string url);
        DataResponse<FileCvQuery> Update(FileCvDto dto, string url);
        DataResponse<FileCvQuery> Delete(Guid id);
        DataResponse<FileCvQuery> GetById(Guid id);
    }
}
