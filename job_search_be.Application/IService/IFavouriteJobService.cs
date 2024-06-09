using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.Banner;
using job_search_be.Domain.Dto.Favourite;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IFavouriteJobService
    {
        DataResponse<List<FavouriteJobDto>> Favourite_Jobs(Guid objId);
        PagedDataResponse<FavouriteJobDto> Favourite_Jobs2(CommonListQuery commonListQuery, Guid objId);

        DataResponse<FavouriteJobDto> Favourite(FavouriteJobDto dto);
        PagedDataResponse<FavoutirJob> Items(CommonListQuery commonList,Guid id);

    }
}
