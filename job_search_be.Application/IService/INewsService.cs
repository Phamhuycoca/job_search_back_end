using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface INewsService
    {
        PagedDataResponse<NewsQuery> Items(CommonListQuery commonList);
        DataResponse<NewsQuery> Create(NewsDto dto);
        DataResponse<NewsQuery> Update(NewsDto dto);
        DataResponse<NewsQuery> Delete(Guid id);
        DataResponse<NewsQuery> GetById(Guid id);
    }
}
