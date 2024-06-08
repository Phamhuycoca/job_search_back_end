using AutoMapper;
using CloudinaryDotNet;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Banner;
using job_search_be.Domain.Dto.City;
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
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public BannerService(IBannerRepository bannerRepository, IMapper mapper,Cloudinary cloudinary)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
        }
        public DataResponse<BannerQuery> Create(BannerDto dto)
        {
            dto.BannerId = Guid.NewGuid();
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            if (dto.file != null)
            {
                dto.BannerPath = upload.ImageUpload(dto.file);
            }
            else
            {
                throw new ApiException(HttpStatusCode.BAD_REQUEST, "Chưa upload ảnh");
            }
            var newData = _bannerRepository.Create(_mapper.Map<Banner>(dto));
            if (newData != null)
            {
                return new DataResponse<BannerQuery>(_mapper.Map<BannerQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<BannerQuery> Delete(Guid id)
        {
            var item = _bannerRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _bannerRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<BannerQuery>(_mapper.Map<BannerQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<BannerQuery> GetById(Guid id)
        {
            var item = _bannerRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<BannerQuery>(_mapper.Map<BannerQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<BannerQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<BannerQuery>>(_bannerRepository.GetAllData());
            var paginatedResult = PaginatedList<BannerQuery>.ToPageList(query.ToList(), commonList.page, commonList.limit);
            return new PagedDataResponse<BannerQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<BannerQuery> Update(BannerDto dto)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            var item = _bannerRepository.GetById(dto.BannerId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            if (dto.file != null)
            {
                if (item.BannerPath!= null)
                {
                    upload.DeleteImage(item.BannerPath);
                }
                dto.BannerPath = upload.ImageUpload(dto.file);
            }

            var newData = _bannerRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<BannerQuery>(_mapper.Map<BannerQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
