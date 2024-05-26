using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.FileCv;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Levelwork;
using job_search_be.Domain.Dto.Profession;
using job_search_be.Domain.Dto.Recruitment;
using job_search_be.Domain.Dto.Salary;
using job_search_be.Domain.Dto.Workexperience;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Repositories;
using job_search_be.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;
        private readonly IFormofworkRepository _formofworkRepository;
        private readonly ILevelworkRepository _levelworkRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ISalaryRepository _aryRepository;
        private readonly IWorkexperienceRepository _workexperienceRepository;
        private readonly IEmployersRepository _employersRepository;
        private readonly IFavoufite_JobRepository _favoufiteJobRepository;
        private readonly IFileCvRepository _fileCvRepository;
        private readonly IRecruitmentRepository _recruitmentRepository;
        public JobService(IJobRepository jobRepository, IMapper mapper,
            IFormofworkRepository formofworkRepository,
            ILevelworkRepository levelworkRepository,
            ICityRepository cityRepository,
            IProfessionRepository professionRepository,
            ISalaryRepository aryRepository,
            IWorkexperienceRepository workexperienceRepository,
            IEmployersRepository employersRepository, 
            IFavoufite_JobRepository favoufiteJobRepository, 
            IFileCvRepository fileCvRepository, 
            IRecruitmentRepository recruitmentRepository)
        {
            _jobRepository = jobRepository;
            _mapper = mapper;
            _formofworkRepository = formofworkRepository;
            _levelworkRepository = levelworkRepository;
            _cityRepository = cityRepository;
            _professionRepository = professionRepository;
            _aryRepository = aryRepository;
            _workexperienceRepository = workexperienceRepository;
            _employersRepository = employersRepository;
            _favoufiteJobRepository = favoufiteJobRepository;
            _fileCvRepository = fileCvRepository;
            _recruitmentRepository = recruitmentRepository;
        }

        public PagedDataResponse<JobQuery> Items(CommonListQuery commonListQuery, Guid objId)
        {
            var query = _mapper.Map<List<JobQuery>>(_jobRepository.GetAllData().Where(x => x.EmployersId == objId).ToList());
            var salaries = _mapper.Map<List<SalaryDto>>(_aryRepository.GetAllData());
            var formofworks = _mapper.Map<List<FormofworkDto>>(_formofworkRepository.GetAllData());
            var levelworks = _mapper.Map<List<LevelworkDto>>(_levelworkRepository.GetAllData());
            var workexperiences = _mapper.Map<List<WorkexperienceDto>>(_workexperienceRepository.GetAllData());
            var professions = _mapper.Map<List<ProfessionDto>>(_professionRepository.GetAllData());
            var cities = _mapper.Map<List<CityDto>>(_cityRepository.GetAllData());
            var items = from jobs in query
                        join salary in salaries on jobs.SalaryId equals salary.SalaryId
                        join
                       formofwork in formofworks on jobs.FormofworkId equals formofwork.FormofworkId
                        join
                       levelwork in levelworks on jobs.LevelworkId equals levelwork.LevelworkId
                        join
                       workexperience in workexperiences on jobs.WorkexperienceId equals workexperience.WorkexperienceId
                        join
                       profession in professions on jobs.ProfessionId equals profession.ProfessionId
                        join
                       city in cities on jobs.CityId equals city.CityId
                        select new JobQuery
                        {
                            JobId = jobs.JobId,
                            JobName = jobs.JobName,
                            RequestJob = jobs.RequestJob,
                            BenefitsJob = jobs.BenefitsJob,
                            AddressJob = jobs.AddressJob,
                            WorkingTime = jobs.WorkingTime,
                            ExpirationDate = jobs.ExpirationDate,
                            WorkexperienceName = workexperience.WorkexperienceName,
                            FormofworkName = formofwork.FormofworkName,
                            CityName = city.CityName,
                            SalaryPrice = salary.SalaryPrice,
                            ProfessionName = profession.ProfessionName,
                            LevelworkName = levelwork.LevelworkName,
                            JobDescription = jobs.JobDescription,
                        };
            if (!string.IsNullOrEmpty(commonListQuery.keyword))
            {
                items = items.Where(x => x.CityName.Contains(commonListQuery.keyword) ||
                x.JobName.Contains(commonListQuery.keyword) ||
                x.SalaryPrice.Contains(commonListQuery.keyword)).ToList();
            }

            var paginatedResult = PaginatedList<JobQuery>.ToPageList(_mapper.Map<List<JobQuery>>(items), commonListQuery.page, commonListQuery.limit);
            return new PagedDataResponse<JobQuery>(paginatedResult, 200, items.Count());
        }

        public DataResponse<JobQuery> Create(JobDto dto)
        {
            dto.JobId = Guid.NewGuid();
            var newData = _jobRepository.Create(_mapper.Map<Job>(dto));
            if (newData != null)
            {
                return new DataResponse<JobQuery>(_mapper.Map<JobQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<JobQuery> Delete(Guid id)
        {
            var item = _jobRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _jobRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<JobQuery>(_mapper.Map<JobQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<JobQuery> GetById(Guid id)
        {
            var item = _jobRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<JobQuery>(_mapper.Map<JobQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }



        public DataResponse<List<JobQuery>> ItemsNoQuery()
        {
            var query = _jobRepository.GetAllData();
            if (query != null && query.Any())
            {
                var cityDtos = _mapper.Map<List<LevelworkDto>>(query);
                return new DataResponse<List<JobQuery>>(_mapper.Map<List<JobQuery>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }

        public DataResponse<JobQuery> Update(JobDto dto)
        {
            var item = _jobRepository.GetById(dto.JobId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var newData = _jobRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<JobQuery>(_mapper.Map<JobQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }

        /*  public PagedDataResponse<JobQueries> ItemsByHome(CommonQueryByHome queryByHome)
          {
              var query = _mapper.Map<List<JobQuery>>(_jobRepository.GetAllData());
              var salaries = _mapper.Map<List<SalaryDto>>(_aryRepository.GetAllData());
              var formofworks = _mapper.Map<List<FormofworkDto>>(_formofworkRepository.GetAllData());
              var levelworks = _mapper.Map<List<LevelworkDto>>(_levelworkRepository.GetAllData());
              var workexperiences = _mapper.Map<List<WorkexperienceDto>>(_workexperienceRepository.GetAllData());
              var professions = _mapper.Map<List<ProfessionDto>>(_professionRepository.GetAllData());
              var cities = _mapper.Map<List<CityDto>>(_cityRepository.GetAllData());
              var employers=_mapper.Map<List<EmployersDto>>(_employersRepository.GetAllData());
              var items = from jobs in query
                          join salary in salaries on jobs.SalaryId equals salary.SalaryId
                          join
                         formofwork in formofworks on jobs.FormofworkId equals formofwork.FormofworkId
                          join
                         levelwork in levelworks on jobs.LevelworkId equals levelwork.LevelworkId
                          join
                         workexperience in workexperiences on jobs.WorkexperienceId equals workexperience.WorkexperienceId
                          join
                         profession in professions on jobs.ProfessionId equals profession.ProfessionId
                          join
                         city in cities on jobs.CityId equals city.CityId
                         join
                         employer in employers on jobs.EmployersId equals employer.EmployersId
                          select new JobQueries
                          {
                              JobId = jobs.JobId,
                              JobName = jobs.JobName,
                              RequestJob = jobs.RequestJob,
                              BenefitsJob = jobs.BenefitsJob,
                              AddressJob = jobs.AddressJob,
                              WorkingTime = jobs.WorkingTime,
                              ExpirationDate = jobs.ExpirationDate,
                              WorkexperienceName = workexperience.WorkexperienceName,
                              FormofworkName = formofwork.FormofworkName,
                              CityName = city.CityName,
                              SalaryPrice = salary.SalaryPrice,
                              ProfessionName = profession.ProfessionName,
                              LevelworkName = levelwork.LevelworkName,
                              EmployersId=jobs.EmployersId,
                              CompanyLogo=employer.CompanyLogo,
                              CompanyName=employer.CompanyName
                          };
              if (!string.IsNullOrEmpty(queryByHome.keyword))
              {
                  items = items.Where(x => x.CityName.Contains(queryByHome.keyword) ||
                  x.JobName.Contains(queryByHome.keyword) ||
                  x.SalaryPrice.Contains(queryByHome.keyword)||
                  x.CityName.Contains(queryByHome.keyword)||
                  x.LevelworkName.Contains(queryByHome.keyword)||
                  x.CompanyName.Contains(queryByHome.keyword)
                  ).ToList();
              }
              if(!string.IsNullOrEmpty(queryByHome.professionId))
              {
                  items = items.Where(x => x.ProfessionId == Guid.Parse(queryByHome.professionId));
              }
              if(!string.IsNullOrEmpty(queryByHome.workexperienceId))
              {
                  items = items.Where(x => x.WorkexperienceId == Guid.Parse(queryByHome.workexperienceId));
              }
              if (!string.IsNullOrEmpty(queryByHome.formofworkId))
              {
                  items = items.Where(x => x.FormofworkId == Guid.Parse(queryByHome.formofworkId));                
              }
              if(!string.IsNullOrEmpty(queryByHome.salaryId))
              {
                  items = items.Where(x => x.SalaryId == Guid.Parse(queryByHome.salaryId));
              }
              if (!string.IsNullOrEmpty(queryByHome.cityId))
              {
                  items = items.Where(x => x.CityId == Guid.Parse(queryByHome.cityId));
              }
              var paginatedResult = PaginatedList<JobQueries>.ToPageList(_mapper.Map<List<JobQueries>>(items), queryByHome.page, queryByHome.limit);
              return new PagedDataResponse<JobQueries>(paginatedResult, 200, items.Count());
          }*/
        public PagedDataResponse<JobQueries> ItemsByHome(CommonQueryByHome queryByHome)
        {
            var query = _jobRepository.GetAllData().AsQueryable();
            var salaries = _aryRepository.GetAllData();
            var formofworks = _formofworkRepository.GetAllData();
            var levelworks = _levelworkRepository.GetAllData();
            var workexperiences = _workexperienceRepository.GetAllData();
            var professions = _professionRepository.GetAllData();
            var cities = _cityRepository.GetAllData();
            var employers = _employersRepository.GetAllData();
            var favourites=_favoufiteJobRepository.GetAllData();
            var items = from jobs in query
                        join salary in salaries on jobs.SalaryId equals salary.SalaryId
                        join formofwork in formofworks on jobs.FormofworkId equals formofwork.FormofworkId
                        join levelwork in levelworks on jobs.LevelworkId equals levelwork.LevelworkId
                        join workexperience in workexperiences on jobs.WorkexperienceId equals workexperience.WorkexperienceId
                        join profession in professions on jobs.ProfessionId equals profession.ProfessionId
                        join city in cities on jobs.CityId equals city.CityId
                        join employer in employers on jobs.EmployersId 
                        equals employer.EmployersId
                        select new JobQueries
                        {
                            JobId = jobs.JobId,
                            JobName = jobs.JobName,
                            RequestJob = jobs.RequestJob,
                            BenefitsJob = jobs.BenefitsJob,
                            AddressJob = jobs.AddressJob,
                            WorkingTime = jobs.WorkingTime,
                            ExpirationDate = jobs.ExpirationDate,
                            WorkexperienceId = jobs.WorkexperienceId,
                            WorkexperienceName = workexperience.WorkexperienceName,
                            FormofworkId = jobs.FormofworkId,
                            FormofworkName = formofwork.FormofworkName,
                            CityId = jobs.CityId,
                            CityName = city.CityName,
                            SalaryId = jobs.SalaryId,
                            SalaryPrice = salary.SalaryPrice,
                            ProfessionId = jobs.ProfessionId,
                            ProfessionName = profession.ProfessionName,
                            LevelworkId = jobs.LevelworkId,
                            LevelworkName = levelwork.LevelworkName,
                            EmployersId = jobs.EmployersId,
                            CompanyLogo = employer.CompanyLogo,
                            CompanyName = employer.CompanyName,
                            JobDescription = jobs.JobDescription,
                        };

            if (!string.IsNullOrEmpty(queryByHome.keyword))
            {
                items = items.Where(x =>
                                         x.JobName.Contains(queryByHome.keyword) ||
                                         x.SalaryPrice.Contains(queryByHome.keyword) ||
                                         x.CityName.Contains(queryByHome.keyword) ||
                                         x.LevelworkName.Contains(queryByHome.keyword) ||
                                         x.CompanyName.Contains(queryByHome.keyword));
            }

            if (!string.IsNullOrEmpty(queryByHome.professionId) && Guid.TryParse(queryByHome.professionId, out var professionId))
            {
                items = items.Where(x => x.ProfessionId.Equals(professionId));
            }
            if (!string.IsNullOrEmpty(queryByHome.levelworkId) && Guid.TryParse(queryByHome.levelworkId, out var levelworkId))
            {
                items = items.Where(x => x.LevelworkId.Equals(levelworkId));
            }
            if (!string.IsNullOrEmpty(queryByHome.workexperienceId) && Guid.TryParse(queryByHome.workexperienceId, out var workexperienceId))
            {
                items = items.Where(x => x.WorkexperienceId == workexperienceId);
            }

            if (!string.IsNullOrEmpty(queryByHome.formofworkId) && Guid.TryParse(queryByHome.formofworkId, out var formofworkId))
            {
                items = items.Where(x => x.FormofworkId == formofworkId);
            }

            if (!string.IsNullOrEmpty(queryByHome.salaryId) && Guid.TryParse(queryByHome.salaryId, out var salaryId))
            {
                items = items.Where(x => x.SalaryId == salaryId);
            }

            if (!string.IsNullOrEmpty(queryByHome.cityId) && Guid.TryParse(queryByHome.cityId, out var cityId))
            {
                items = items.Where(x => x.CityId == cityId);
            }

            var paginatedResult = PaginatedList<JobQueries>.ToPageList(items.ToList(), queryByHome.page, queryByHome.limit);
            return new PagedDataResponse<JobQueries>(paginatedResult, 200, items.Count());
        }

        public DataResponse<JobQueries> ItemById(Guid id)
        {
            var query = _jobRepository.GetAllData().AsQueryable();
            var salaries = _aryRepository.GetAllData();
            var formofworks = _formofworkRepository.GetAllData();
            var levelworks = _levelworkRepository.GetAllData();
            var workexperiences = _workexperienceRepository.GetAllData();
            var professions = _professionRepository.GetAllData();
            var cities = _cityRepository.GetAllData();
            var employers = _employersRepository.GetAllData();

            var item = from jobs in query
                       join salary in salaries on jobs.SalaryId equals salary.SalaryId
                       join formofwork in formofworks on jobs.FormofworkId equals formofwork.FormofworkId
                       join levelwork in levelworks on jobs.LevelworkId equals levelwork.LevelworkId
                       join workexperience in workexperiences on jobs.WorkexperienceId equals workexperience.WorkexperienceId
                       join profession in professions on jobs.ProfessionId equals profession.ProfessionId
                       join city in cities on jobs.CityId equals city.CityId
                       join employer in employers on jobs.EmployersId equals employer.EmployersId
                       where jobs.JobId == id
                       select new JobQueries
                       {
                           JobId = jobs.JobId,
                           JobName = jobs.JobName,
                           RequestJob = jobs.RequestJob,
                           BenefitsJob = jobs.BenefitsJob,
                           AddressJob = jobs.AddressJob,
                           WorkingTime = jobs.WorkingTime,
                           ExpirationDate = jobs.ExpirationDate,
                           WorkexperienceId = jobs.WorkexperienceId,
                           WorkexperienceName = workexperience.WorkexperienceName,
                           FormofworkId = jobs.FormofworkId,
                           FormofworkName = formofwork.FormofworkName,
                           CityId = jobs.CityId,
                           CityName = city.CityName,
                           SalaryId = jobs.SalaryId,
                           SalaryPrice = salary.SalaryPrice,
                           ProfessionId = jobs.ProfessionId,
                           ProfessionName = profession.ProfessionName,
                           LevelworkId = jobs.LevelworkId,
                           LevelworkName = levelwork.LevelworkName,
                           EmployersId = jobs.EmployersId,
                           CompanyLogo = employer.CompanyLogo,
                           CompanyName = employer.CompanyName,
                           JobDescription = jobs.JobDescription,
                       };

            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }

            return new DataResponse<JobQueries>(item.SingleOrDefault(), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<JobQueries> RelatedJobs(CommonQueryByHome queryByHome, Guid id)
        {
            var query = _jobRepository.GetAllData().AsQueryable();
            var salaries = _aryRepository.GetAllData();
            var formofworks = _formofworkRepository.GetAllData();
            var levelworks = _levelworkRepository.GetAllData();
            var workexperiences = _workexperienceRepository.GetAllData();
            var professions = _professionRepository.GetAllData();
            var cities = _cityRepository.GetAllData();
            var employers = _employersRepository.GetAllData();

            var items = from jobs in query
                        join salary in salaries on jobs.SalaryId equals salary.SalaryId
                        join formofwork in formofworks on jobs.FormofworkId equals formofwork.FormofworkId
                        join levelwork in levelworks on jobs.LevelworkId equals levelwork.LevelworkId
                        join workexperience in workexperiences on jobs.WorkexperienceId equals workexperience.WorkexperienceId
                        join profession in professions on jobs.ProfessionId equals profession.ProfessionId
                        join city in cities on jobs.CityId equals city.CityId
                        join employer in employers on jobs.EmployersId equals employer.EmployersId
                        select new JobQueries
                        {
                            JobId = jobs.JobId,
                            JobName = jobs.JobName,
                            RequestJob = jobs.RequestJob,
                            BenefitsJob = jobs.BenefitsJob,
                            AddressJob = jobs.AddressJob,
                            WorkingTime = jobs.WorkingTime,
                            ExpirationDate = jobs.ExpirationDate,
                            WorkexperienceId = jobs.WorkexperienceId,
                            WorkexperienceName = workexperience.WorkexperienceName,
                            FormofworkId = jobs.FormofworkId,
                            FormofworkName = formofwork.FormofworkName,
                            CityId = jobs.CityId,
                            CityName = city.CityName,
                            SalaryId = jobs.SalaryId,
                            SalaryPrice = salary.SalaryPrice,
                            ProfessionId = jobs.ProfessionId,
                            ProfessionName = profession.ProfessionName,
                            LevelworkId = jobs.LevelworkId,
                            LevelworkName = levelwork.LevelworkName,
                            EmployersId = jobs.EmployersId,
                            CompanyLogo = employer.CompanyLogo,
                            CompanyName = employer.CompanyName,
                            JobDescription = jobs.JobDescription,

                        };

            items = items.Where(x => x.JobId != id);

            if (!string.IsNullOrEmpty(queryByHome.keyword))
            {
                items = items.Where(x => x.JobName.Contains(queryByHome.keyword) ||
                                         x.SalaryPrice.Contains(queryByHome.keyword) ||
                                         x.CityName.Contains(queryByHome.keyword) ||
                                         x.LevelworkName.Contains(queryByHome.keyword) ||
                                         x.CompanyName.Contains(queryByHome.keyword));
            }


            if (!string.IsNullOrEmpty(queryByHome.professionId) && Guid.TryParse(queryByHome.professionId, out var professionId))
            {
                items = items.Where(x => x.ProfessionId.Equals(professionId));
            }
            if (!string.IsNullOrEmpty(queryByHome.levelworkId) && Guid.TryParse(queryByHome.levelworkId, out var levelworkId))
            {
                items = items.Where(x => x.LevelworkId.Equals(levelworkId));
            }
            if (!string.IsNullOrEmpty(queryByHome.workexperienceId) && Guid.TryParse(queryByHome.workexperienceId, out var workexperienceId))
            {
                items = items.Where(x => x.WorkexperienceId == workexperienceId);
            }

            if (!string.IsNullOrEmpty(queryByHome.formofworkId) && Guid.TryParse(queryByHome.formofworkId, out var formofworkId))
            {
                items = items.Where(x => x.FormofworkId == formofworkId);
            }

            if (!string.IsNullOrEmpty(queryByHome.salaryId) && Guid.TryParse(queryByHome.salaryId, out var salaryId))
            {
                items = items.Where(x => x.SalaryId == salaryId);
            }

            if (!string.IsNullOrEmpty(queryByHome.cityId) && Guid.TryParse(queryByHome.cityId, out var cityId))
            {
                items = items.Where(x => x.CityId == cityId);
            }

            var paginatedResult = PaginatedList<JobQueries>.ToPageList(items.ToList(), queryByHome.page, queryByHome.limit);
            return new PagedDataResponse<JobQueries>(paginatedResult, 200, items.Count());
        }

        public PagedDataResponse<JobQueries> ItemsByHomeById(CommonQueryByHome queryByHome, Guid id)
        {
            var query = _jobRepository.GetAllData().AsQueryable();
            var salaries = _aryRepository.GetAllData();
            var formofworks = _formofworkRepository.GetAllData();
            var levelworks = _levelworkRepository.GetAllData();
            var workexperiences = _workexperienceRepository.GetAllData();
            var professions = _professionRepository.GetAllData();
            var cities = _cityRepository.GetAllData();
            var employers = _employersRepository.GetAllData();
            var items = from jobs in query
                        join salary in salaries on jobs.SalaryId equals salary.SalaryId
                        join formofwork in formofworks on jobs.FormofworkId equals formofwork.FormofworkId
                        join levelwork in levelworks on jobs.LevelworkId equals levelwork.LevelworkId
                        join workexperience in workexperiences on jobs.WorkexperienceId equals workexperience.WorkexperienceId
                        join profession in professions on jobs.ProfessionId equals profession.ProfessionId
                        join city in cities on jobs.CityId equals city.CityId
                        join employer in employers on jobs.EmployersId equals employer.EmployersId
                        select new JobQueries
                        {
                            JobId = jobs.JobId,
                            JobName = jobs.JobName,
                            RequestJob = jobs.RequestJob,
                            BenefitsJob = jobs.BenefitsJob,
                            AddressJob = jobs.AddressJob,
                            WorkingTime = jobs.WorkingTime,
                            ExpirationDate = jobs.ExpirationDate,
                            WorkexperienceId = jobs.WorkexperienceId,
                            WorkexperienceName = workexperience.WorkexperienceName,
                            FormofworkId = jobs.FormofworkId,
                            FormofworkName = formofwork.FormofworkName,
                            CityId = jobs.CityId,
                            CityName = city.CityName,
                            SalaryId = jobs.SalaryId,
                            SalaryPrice = salary.SalaryPrice,
                            ProfessionId = jobs.ProfessionId,
                            ProfessionName = profession.ProfessionName,
                            LevelworkId = jobs.LevelworkId,
                            LevelworkName = levelwork.LevelworkName,
                            EmployersId = jobs.EmployersId,
                            CompanyLogo = employer.CompanyLogo,
                            CompanyName = employer.CompanyName,
                            JobDescription = jobs.JobDescription,
                           Job_SeekerId=id
                        };

            if (!string.IsNullOrEmpty(queryByHome.keyword))
            {
                items = items.Where(x =>
                                         x.JobName.Contains(queryByHome.keyword) ||
                                         x.SalaryPrice.Contains(queryByHome.keyword) ||
                                         x.CityName.Contains(queryByHome.keyword) ||
                                         x.LevelworkName.Contains(queryByHome.keyword) ||
                                         x.CompanyName.Contains(queryByHome.keyword));
            }

            if (!string.IsNullOrEmpty(queryByHome.professionId) && Guid.TryParse(queryByHome.professionId, out var professionId))
            {
                items = items.Where(x => x.ProfessionId.Equals(professionId));
            }
            if (!string.IsNullOrEmpty(queryByHome.levelworkId) && Guid.TryParse(queryByHome.levelworkId, out var levelworkId))
            {
                items = items.Where(x => x.LevelworkId.Equals(levelworkId));
            }
            if (!string.IsNullOrEmpty(queryByHome.workexperienceId) && Guid.TryParse(queryByHome.workexperienceId, out var workexperienceId))
            {
                items = items.Where(x => x.WorkexperienceId == workexperienceId);
            }

            if (!string.IsNullOrEmpty(queryByHome.formofworkId) && Guid.TryParse(queryByHome.formofworkId, out var formofworkId))
            {
                items = items.Where(x => x.FormofworkId == formofworkId);
            }

            if (!string.IsNullOrEmpty(queryByHome.salaryId) && Guid.TryParse(queryByHome.salaryId, out var salaryId))
            {
                items = items.Where(x => x.SalaryId == salaryId);
            }

            if (!string.IsNullOrEmpty(queryByHome.cityId) && Guid.TryParse(queryByHome.cityId, out var cityId))
            {
                items = items.Where(x => x.CityId == cityId);
            }

            var paginatedResult = PaginatedList<JobQueries>.ToPageList(items.ToList(), queryByHome.page, queryByHome.limit);
            return new PagedDataResponse<JobQueries>(paginatedResult, 200, items.Count());
        }

       
    }
}
