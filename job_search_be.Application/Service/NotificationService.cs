using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Notification;
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
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly IJob_SeekerRepository _job_SeekerRepository;
        private readonly IJobRepository _jobRepository;
        public NotificationService(INotificationRepository notificationRepository, IMapper mapper, IJob_SeekerRepository job_SeekerRepository, IJobRepository jobRepository)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _job_SeekerRepository = job_SeekerRepository;
            _jobRepository = jobRepository;
        }
        public DataResponse<NotificationQuery> Create(NotificationDto dto)
        {
            dto.NotificationId = Guid.NewGuid();
            var nameJob = _jobRepository.GetById(dto.JobId);
            var nameJOb_Seeker = _job_SeekerRepository.GetById(dto.Job_SeekerId);
            dto.Message = $"{nameJOb_Seeker.FullName} đã ứng tuyển công việc "+$"{nameJob.JobName} này";
            var newData = _notificationRepository.Create(_mapper.Map<Notification>(dto));
            if (newData != null)
            {
                return new DataResponse<NotificationQuery>(_mapper.Map<NotificationQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<NotificationQuery> Delete(Guid id)
        {
            var item = _notificationRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _notificationRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<NotificationQuery>(_mapper.Map<NotificationQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<NotificationQuery> GetById(Guid id)
        {
            var item = _notificationRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<NotificationQuery>(_mapper.Map<NotificationQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<NotificationQuery> Items(CommonListQuery commonList,Guid id)
        {
            var query = _mapper.Map<List<NotificationQuery>>(_notificationRepository.GetAllData().Where(x=>x.Job_SeekerId==id &&x.EmployersId==null));
           
            var paginatedResult = PaginatedList<NotificationQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<NotificationQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<NotificationDto>> ItemsList()
        {
            var query = _notificationRepository.GetAllData();
            if (query != null && query.Any())
            {
                var cityDtos = _mapper.Map<List<NotificationDto>>(query);
                return new DataResponse<List<NotificationDto>>(_mapper.Map<List<NotificationDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<List<NotificationQuery>> ItemsListByEmployer(Guid id)
        {
            var query = _notificationRepository.GetAllData();
            if (query != null && query.Any())
            {
                var cityDtos = _mapper.Map<List<NotificationDto>>(query);
                return new DataResponse<List<NotificationQuery>>(_mapper.Map<List<NotificationQuery >>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<NotificationQuery> Update(NotificationDto dto)
        {
            var item = _notificationRepository.GetById(dto.NotificationId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _notificationRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<NotificationQuery>(_mapper.Map<NotificationQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
