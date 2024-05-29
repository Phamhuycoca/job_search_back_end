using AutoMapper;
using CloudinaryDotNet;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Domain.Dto.News;
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
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public NewsService(INewsRepository newsRepository, IMapper mapper,Cloudinary cloudinary)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }

        public DataResponse<NewsQuery> Create(NewsDto dto)
        {
            dto.NewsId = Guid.NewGuid();
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            if (dto.file != null)
            {
                dto.PathImage= upload.ImageUpload(dto.file);
            }
            var newData = _newsRepository.Create(_mapper.Map<News>(dto));
            if (newData != null)
            {
                return new DataResponse<NewsQuery>(_mapper.Map<NewsQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<NewsQuery> Delete(Guid id)
        {
            var item = _newsRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<NewsQuery>(_mapper.Map<NewsQuery >(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public DataResponse<NewsQuery> GetById(Guid id)
        {
            var item = _newsRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<NewsQuery>(_mapper.Map<NewsQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<NewsQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<NewsQuery>>(_newsRepository.GetAllData());
            var paginatedResult = PaginatedList<NewsQuery>.ToPageList(query.ToList(), commonList.page, commonList.limit);
            return new PagedDataResponse<NewsQuery>(paginatedResult, 200, query.Count());
        }

       
        public DataResponse<NewsQuery> Update(NewsDto dto)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            var item = _newsRepository.GetById(dto.NewsId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            if (dto.file != null)
            {
                if (item.PathImage != null)
                {
                    upload.DeleteImage(item.PathImage);
                }
                dto.PathImage = upload.ImageUpload(dto.file);
            }

            var newData = _newsRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<NewsQuery>(_mapper.Map<NewsQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
