using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Domain.Dto.Recruitment;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Repositories;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IRecruitmentRepository _recruitmentRepository;
        private readonly IMapper _mapper;
        private readonly IEmployersRepository _employersRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IJob_SeekerRepository _jobSeekerRepository;
        public RecruitmentService(IRecruitmentRepository recruitmentRepository, IMapper mapper, IEmployersRepository employersRepository, IJobRepository jobRepository, IJob_SeekerRepository job_SeekerRepository)
        {
            _recruitmentRepository = recruitmentRepository;
            _mapper = mapper;
            _employersRepository = employersRepository;
            _jobRepository = jobRepository;
            _jobSeekerRepository = job_SeekerRepository;
        }

        public DataResponse<RecruitmentList> ChangeFeedback(RecruitmentChangeFeedback changeFeedback)
        {
            var item = _recruitmentRepository.GetById(changeFeedback.RecruitmentId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _recruitmentRepository.Update(_mapper.Map(changeFeedback, item));
            if (newData != null)
            {
                return new DataResponse<RecruitmentList>(_mapper.Map<RecruitmentList>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }

        public DataResponse<RecruitmentList> ChangeStatus(RecruitmentChangeStatus changeStatus)
        {
            var item = _recruitmentRepository.GetById(changeStatus.RecruitmentId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _recruitmentRepository.Update(_mapper.Map(changeStatus, item));
            if (newData != null)
            {
                return new DataResponse<RecruitmentList>(_mapper.Map<RecruitmentList>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }

        public DataResponse<RecruitmentQuery> Create(RecruitmentDto dto)
        {
            dto.RecruitmentId = Guid.NewGuid();
            dto.IsStatus = false;
            //dto.RecruitmentDateTime = DateTime.Today.Date.ToString("dd/MM/yyyy dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            dto.RecruitmentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            /* var checkJob = _recruitmentRepository.GetAllData().Where(x => x.JobId == dto.JobId).SingleOrDefault();
             if (checkJob != null)
             {
                 throw new ApiException(HttpStatusCode.BAD_REQUEST, "Công việc đã được ứng tuyển");
             }*/
            var newData = _recruitmentRepository.Create(_mapper.Map<Recruitment>(dto));
            if (newData != null)
            {
                return new DataResponse<RecruitmentQuery>(_mapper.Map<RecruitmentQuery>(newData), HttpStatusCode.OK, "Ứng tuyển công việc thành công");
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, "Ứng tuyển công việc thất bại");
        }

        public DataResponse<RecruitmentQuery> Delete(Guid id)
        {
            var item = _recruitmentRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _recruitmentRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<RecruitmentQuery>(_mapper.Map<RecruitmentQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<RecruitmentQuery> GetById(Guid id)
        {
            var item = _recruitmentRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<RecruitmentQuery>(_mapper.Map<RecruitmentQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<RecruitmentQuery> Items(CommonListQuery commonList, Guid id)
        {
            var query = _mapper.Map<List<RecruitmentQuery>>(_recruitmentRepository.GetAllData().Where(e => e.Job_SeekerId == id));
            var paginatedResult = PaginatedList<RecruitmentQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<RecruitmentQuery>(paginatedResult, 200, query.Count());
        }

        public PagedDataResponse<RecruitmentList> ItemsByEmployer(CommonQueryByHome commonList, Guid id)
        {
            //var query = _mapper.Map<List<RecruitmentList>>(_recruitmentRepository.GetAllData().Where(e => e.EmployersId == id));
            var recruiments = _mapper.Map<List<RecruitmentQuery>>(_recruitmentRepository.GetAllData().Where(x=>x.IsFeedback==null ||x.IsFeedback==false));
            var jobs = _mapper.Map<List<JobQuery>>(_jobRepository.GetAllData());
            var jobseekers = _mapper.Map<List<Job_SeekerQuery>>(_jobSeekerRepository.GetAllData());
            var query = from recruiment in recruiments
                        join
                        job in jobs on recruiment.JobId
                        equals job.JobId
                        join
                        jobseeker in jobseekers on recruiment.Job_SeekerId equals jobseeker.Job_SeekerId
                        where recruiment.JobId == id 
                        select new RecruitmentList
                        {
                            JobId = job.JobId,
                            Avatar =jobseeker.Avatar,
                            FullName=jobseeker.FullName,
                            Content=recruiment.Content,
                            EmployersId=recruiment.EmployersId,
                            IsStatus=recruiment.IsStatus,
                            JobName=job.JobName,
                            Job_Cv=jobseeker.Job_Cv,
                            Job_SeekerId = recruiment.Job_SeekerId,
                            RecruitmentDateTime=recruiment.RecruitmentDateTime,
                            RecruitmentId = recruiment.RecruitmentId,
                            IsFeedback=recruiment.IsFeedback,
                            FormofworkId=job.FormofworkId,
                            ProfessionId=job.ProfessionId,
                            WorkexperienceId = job.WorkexperienceId
                        };
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x =>
                                         x.JobName.Contains(commonList.keyword));
            }
            if (!string.IsNullOrEmpty(commonList.professionId) && Guid.TryParse(commonList.professionId, out var professionId))
            {
                query = query.Where(x => x.ProfessionId.Equals(professionId));
            }

            if (!string.IsNullOrEmpty(commonList.workexperienceId) && Guid.TryParse(commonList.workexperienceId, out var workexperienceId))
            {
                query = query.Where(x => x.WorkexperienceId == workexperienceId);
            }

            if (!string.IsNullOrEmpty(commonList.formofworkId) && Guid.TryParse(commonList.formofworkId, out var formofworkId))
            {
                query = query.Where(x => x.FormofworkId == formofworkId);
            }

            var paginatedResult = PaginatedList<RecruitmentList>.ToPageList(_mapper.Map<List<RecruitmentList>>(query), commonList.page, commonList.limit);
            return new PagedDataResponse<RecruitmentList>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<RecruitmentQuery>> ItemsByEmployerId(Guid id)
        {
            var query = _recruitmentRepository.GetAllData().Where(x=>x.EmployersId==id && x.IsFeedback == null || x.IsFeedback == false);
            if (query != null && query.Any())
            {
                var cityDtos = _mapper.Map<List<RecruitmentQuery>>(query);
                return new DataResponse<List<RecruitmentQuery>>(_mapper.Map<List<RecruitmentQuery>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public PagedDataResponse<RecruitmentList> ItemsByEmployerSuitable(CommonQueryByHome commonList, Guid id)
        {
            var recruiments = _mapper.Map<List<RecruitmentQuery>>(_recruitmentRepository.GetAllData());
            var jobs = _mapper.Map<List<JobQuery>>(_jobRepository.GetAllData());
            var jobseekers = _mapper.Map<List<Job_SeekerQuery>>(_jobSeekerRepository.GetAllData());
            var query = from recruiment in recruiments
                        join
                        job in jobs on recruiment.JobId
                        equals job.JobId
                        join
                        jobseeker in jobseekers on recruiment.Job_SeekerId equals jobseeker.Job_SeekerId
                        where recruiment.EmployersId == id && recruiment.IsFeedback == true
                        select new RecruitmentList
                        {
                            JobId = job.JobId,
                            Avatar = jobseeker.Avatar,
                            FullName = jobseeker.FullName,
                            Content = recruiment.Content,
                            EmployersId = recruiment.EmployersId,
                            IsStatus = recruiment.IsStatus,
                            JobName = job.JobName,
                            Job_Cv = jobseeker.Job_Cv,
                            Job_SeekerId = recruiment.Job_SeekerId,
                            RecruitmentDateTime = recruiment.RecruitmentDateTime,
                            RecruitmentId = recruiment.RecruitmentId,
                            IsFeedback = recruiment.IsFeedback,
                            FormofworkId = job.FormofworkId,
                            ProfessionId = job.ProfessionId,
                            WorkexperienceId = job.WorkexperienceId
                        };
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x =>
                                         x.JobName.Contains(commonList.keyword));
            }
            if (!string.IsNullOrEmpty(commonList.professionId) && Guid.TryParse(commonList.professionId, out var professionId))
            {
                query = query.Where(x => x.ProfessionId.Equals(professionId));
            }

            if (!string.IsNullOrEmpty(commonList.workexperienceId) && Guid.TryParse(commonList.workexperienceId, out var workexperienceId))
            {
                query = query.Where(x => x.WorkexperienceId == workexperienceId);
            }

            if (!string.IsNullOrEmpty(commonList.formofworkId) && Guid.TryParse(commonList.formofworkId, out var formofworkId))
            {
                query = query.Where(x => x.FormofworkId == formofworkId);
            }

            var paginatedResult = PaginatedList<RecruitmentList>.ToPageList(_mapper.Map<List<RecruitmentList>>(query), commonList.page, commonList.limit);
            return new PagedDataResponse<RecruitmentList>(paginatedResult, 200, query.Count());
        }

        public PagedDataResponse<RecruitmentList> ItemsByJob_seeker(CommonListQuery commonList, Guid id)
        {
            //var query = _mapper.Map<List<RecruitmentList>>(_recruitmentRepository.GetAllData().Where(e => e.EmployersId == id));
            var recruiments = _mapper.Map<List<RecruitmentQuery>>(_recruitmentRepository.GetAllData());
            var jobs = _mapper.Map<List<JobQuery>>(_jobRepository.GetAllData());
            var jobseekers = _mapper.Map<List<Job_SeekerQuery>>(_jobSeekerRepository.GetAllData());
            var employers = _mapper.Map<List<EmployersQuery>>(_employersRepository.GetAllData());
            var query = from recruiment in recruiments
                        join
                        job in jobs on recruiment.JobId
                        equals job.JobId
                        join
                        jobseeker in jobseekers on recruiment.Job_SeekerId equals jobseeker.Job_SeekerId
                        join 
                        employer in employers on job.EmployersId equals employer.EmployersId
                        where recruiment.Job_SeekerId == id 
                        //&& recruiment.IsFeedback == false
                        select new RecruitmentList
                        {
                            JobId = job.JobId,
                            Avatar = jobseeker.Avatar,
                            FullName = jobseeker.FullName,
                            Content = recruiment.Content,
                            EmployersId = recruiment.EmployersId,
                            IsStatus = recruiment.IsStatus,
                            JobName = job.JobName,
                            Job_Cv = jobseeker.Job_Cv,
                            Job_SeekerId = recruiment.Job_SeekerId,
                            RecruitmentDateTime = recruiment.RecruitmentDateTime,
                            RecruitmentId = recruiment.RecruitmentId,
                            IsFeedback = recruiment.IsFeedback,
                            CompanyLogo=employer.CompanyLogo

                        };
            var paginatedResult = PaginatedList<RecruitmentList>.ToPageList(_mapper.Map<List<RecruitmentList>>(query), commonList.page, commonList.limit);
            return new PagedDataResponse<RecruitmentList>(paginatedResult, 200, query.Count());
        }

        public PagedDataResponse<RecruitmentList> Job_SeekerById(CommonQueryByHome commonList, Guid id)
        {
            var recruiments = _mapper.Map<List<RecruitmentQuery>>(_recruitmentRepository.GetAllData());
            var jobs = _mapper.Map<List<JobQuery>>(_jobRepository.GetAllData());
            var jobseekers = _mapper.Map<List<Job_SeekerQuery>>(_jobSeekerRepository.GetAllData());
            var query = from recruiment in recruiments
                        join
                        job in jobs on recruiment.JobId
                        equals job.JobId
                        join
                        jobseeker in jobseekers on recruiment.Job_SeekerId equals jobseeker.Job_SeekerId
                        where recruiment.JobId == id && recruiment.IsStatus == true
                        select new RecruitmentList
                        {
                            JobId = job.JobId,
                            Avatar = jobseeker.Avatar,
                            FullName = jobseeker.FullName,
                            Content = recruiment.Content,
                            EmployersId = recruiment.EmployersId,
                            IsStatus = recruiment.IsStatus,
                            JobName = job.JobName,
                            Job_Cv = jobseeker.Job_Cv,
                            Job_SeekerId = recruiment.Job_SeekerId,
                            RecruitmentDateTime = recruiment.RecruitmentDateTime,
                            RecruitmentId = recruiment.RecruitmentId,
                            IsFeedback = recruiment.IsFeedback,
                            FormofworkId = job.FormofworkId,
                            ProfessionId = job.ProfessionId,
                            WorkexperienceId = job.WorkexperienceId
                        };
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x =>
                                         x.JobName.Contains(commonList.keyword));
            }
            if (!string.IsNullOrEmpty(commonList.professionId) && Guid.TryParse(commonList.professionId, out var professionId))
            {
                query = query.Where(x => x.ProfessionId.Equals(professionId));
            }

            if (!string.IsNullOrEmpty(commonList.workexperienceId) && Guid.TryParse(commonList.workexperienceId, out var workexperienceId))
            {
                query = query.Where(x => x.WorkexperienceId == workexperienceId);
            }

            if (!string.IsNullOrEmpty(commonList.formofworkId) && Guid.TryParse(commonList.formofworkId, out var formofworkId))
            {
                query = query.Where(x => x.FormofworkId == formofworkId);
            }

            var paginatedResult = PaginatedList<RecruitmentList>.ToPageList(_mapper.Map<List<RecruitmentList>>(query), commonList.page, commonList.limit);
            return new PagedDataResponse<RecruitmentList>(paginatedResult, 200, query.Count());
        }

        public DataResponse<RecruitmentQuery> Update(RecruitmentDto dto)
        {
            var item = _recruitmentRepository.GetById(dto.RecruitmentId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _recruitmentRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<RecruitmentQuery>(_mapper.Map<RecruitmentQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }
    }
}
