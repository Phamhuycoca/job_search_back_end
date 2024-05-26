using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Favourite;
using job_search_be.Domain.Dto.Job;
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
    public class FavouriteJobService:IFavouriteJobService
    {
        private readonly IFavoufite_JobRepository _favoufite_JobRepository;
        private readonly IMapper _mapper;
        private readonly IJobRepository _jobRepository;
        public FavouriteJobService(IFavoufite_JobRepository favoufite_JobRepository, IMapper mapper, IJobRepository jobRepository)
        {
            _favoufite_JobRepository = favoufite_JobRepository;
            _mapper = mapper;
            _jobRepository = jobRepository;
        }

        public DataResponse<FavouriteJobDto> Favourite(FavouriteJobDto dto)
        {
            var obj=new Favoufite_Job();
            var isFavourite = _favoufite_JobRepository.GetAllData().Where(x => x.Favoufite_Job_Id == dto.Favoufite_Job_Id && x.Job_SeekerId == dto.Job_SeekerId).FirstOrDefault();
            if(isFavourite == null)
            {
                obj=_favoufite_JobRepository.Create(_mapper.Map<Favoufite_Job>(dto));
            }
            else
            {
                obj = _favoufite_JobRepository.Update(_mapper.Map<Favoufite_Job>(dto));
            }
            return new DataResponse<FavouriteJobDto>(_mapper.Map<FavouriteJobDto>(obj), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);

        }

        public DataResponse<List<FavouriteJobDto>> Favourite_Jobs(Guid objId)
        {
            var query = _favoufite_JobRepository.GetAllData().Where(x => x.Job_SeekerId == objId);
                return new DataResponse<List<FavouriteJobDto>>(_mapper.Map<List<FavouriteJobDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);

        }

        public PagedDataResponse<FavouriteJobDto> Favourite_Jobs2(CommonListQuery commonListQuery, Guid objId)
        {
            var query = _mapper.Map<List<FavouriteJobDto>>(_favoufite_JobRepository.GetAllData().Where(x => x.Job_SeekerId == objId));
            var total = _jobRepository.GetAllData().Count();
            commonListQuery.limit = total;
            var paginatedResult = PaginatedList<FavouriteJobDto>.ToPageList(query, commonListQuery.page, commonListQuery.limit);
            return new PagedDataResponse<FavouriteJobDto>(paginatedResult, 200, query.Count());
        }

        /* public DataResponse<List<FavouriteJobDto>> Favourite_Jobs(Guid objId)
         {
             *//*var query = _mapper.Map<List<FavouriteJobDto>>(_favoufite_JobRepository.GetAllData().Where(x=>x.Job_SeekerId==objId));
             var total = _jobRepository.GetAllData().Count();
             commonListQuery.limit = total;
             var paginatedResult = PaginatedList<FavouriteJobDto>.ToPageList(query, commonListQuery.page,commonListQuery.limit);
             return new PagedDataResponse<FavouriteJobDto>(paginatedResult, 200, query.Count());*//*
             var query = _favoufite_JobRepository.GetAllData().Where(x => x.Job_SeekerId == objId);
             if (query != null && query.Any())
             {
                 return new DataResponse<List<FavouriteJobDto>>(_mapper.Map<List<FavouriteJobDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
             }
             throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
         }*/

        /* public PagedDataResponse<FavouriteJobDto> Favourite_Jobs(CommonListQuery commonListQuery, Guid objId)
         {
             var query = _mapper.Map<List<FavouriteJobDto>>(_favoufite_JobRepository.GetAllData().Where(x => x.Job_SeekerId == objId));
             var total = _jobRepository.GetAllData().Count();
             commonListQuery.limit = total;
             var paginatedResult = PaginatedList<FavouriteJobDto>.ToPageList(query, commonListQuery.page, commonListQuery.limit);
             return new PagedDataResponse<FavouriteJobDto>(paginatedResult, 200, query.Count());
         }
 */

    }
}
