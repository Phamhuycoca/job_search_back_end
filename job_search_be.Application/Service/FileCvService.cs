using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.FileCv;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Job_Seeker;
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
    public class FileCvService : IFileCvService
    {
        private readonly IFileCvRepository _fileCvRepository;
        private readonly IMapper _mapper;
        public FileCvService(IFileCvRepository fileCvRepository, IMapper mapper)
        {
            _fileCvRepository = fileCvRepository;
            _mapper= mapper;
        }
        public DataResponse<FileCvQuery> Create(FileCvDto dto,string url)
        {
            dto.FileCvId = Guid.NewGuid();
            dto.HostPath = url;
            if (dto.pdfFile == null)
            {
                throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
            }
            dto.FileCvPath= url + FileUploadService.CreatePDF(dto.pdfFile);
            var newData = _fileCvRepository.Create(_mapper.Map<FileCv>(dto));
            if (newData != null)
            {
                return new DataResponse<FileCvQuery>(_mapper.Map<FileCvQuery>(newData), HttpStatusCode.OK, "Lưu thông tin Cv của bạn thành công");
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
            throw new NotImplementedException();
        }

        public DataResponse<FileCvQuery> Delete(Guid id)
        {
            var item = _fileCvRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _fileCvRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<FileCvQuery>(_mapper.Map<FileCvQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<FileCvQuery> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public PagedDataResponse<FileCvQuery> Items(CommonListQuery commonListQuery, Guid id)
        {
            var query = _mapper.Map<List<FileCvQuery>>(_fileCvRepository.GetAllData().Where(x=>x.Job_SeekerId==id));         
            var paginatedResult = PaginatedList<FileCvQuery>.ToPageList(query, commonListQuery.page, commonListQuery.limit);
            return new PagedDataResponse<FileCvQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<FileCvQuery> Update(FileCvDto dto, string url)
        {
            throw new NotImplementedException();
        }
    }
}
