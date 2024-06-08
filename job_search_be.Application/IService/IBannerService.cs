using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Banner;
using job_search_be.Domain.Dto.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IBannerService
    {
        PagedDataResponse<BannerQuery> Items(CommonListQuery commonList);
        DataResponse<BannerQuery> Create(BannerDto dto);
        DataResponse<BannerQuery> Update(BannerDto dto);
        DataResponse<BannerQuery> Delete(Guid id);
        DataResponse<BannerQuery> GetById(Guid id);

    }
}
