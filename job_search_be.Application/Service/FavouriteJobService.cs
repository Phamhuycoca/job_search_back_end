using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Favourite;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Levelwork;
using job_search_be.Domain.Dto.Profession;
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
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class FavouriteJobService:IFavouriteJobService
    {
        private readonly IFavoufite_JobRepository _favoufite_JobRepository;
        private readonly IMapper _mapper;
        private readonly IJobRepository _jobRepository;
        private readonly IFormofworkRepository _formofworkRepository;
        private readonly ILevelworkRepository _levelworkRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ISalaryRepository _aryRepository;
        private readonly IWorkexperienceRepository _workexperienceRepository;
        private readonly IEmployersRepository _employersRepository;

        public FavouriteJobService(IFavoufite_JobRepository favoufite_JobRepository, IMapper mapper, IJobRepository jobRepository, IFormofworkRepository formofworkRepository,
            ILevelworkRepository levelworkRepository,
            ICityRepository cityRepository,
            IProfessionRepository professionRepository,
            ISalaryRepository aryRepository,
            IWorkexperienceRepository workexperienceRepository,
            IEmployersRepository employersRepository)
        {
            _favoufite_JobRepository = favoufite_JobRepository;
            _mapper = mapper;
            _jobRepository = jobRepository;
            _formofworkRepository = formofworkRepository;
            _levelworkRepository = levelworkRepository;
            _cityRepository = cityRepository;
            _professionRepository = professionRepository;
            _aryRepository = aryRepository;
            _workexperienceRepository = workexperienceRepository;
            _employersRepository = employersRepository;
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

        public PagedDataResponse<FavoutirJob> Items(CommonListQuery commonList, Guid id)
        {
            var f=_favoufite_JobRepository.GetAllData().Where(x=>x.Job_SeekerId==id);
            var job = _jobRepository.GetAllData().ToList();
            var salaries = _mapper.Map<List<SalaryDto>>(_aryRepository.GetAllData());
            var formofworks = _mapper.Map<List<FormofworkDto>>(_formofworkRepository.GetAllData());
            var levelworks = _mapper.Map<List<LevelworkDto>>(_levelworkRepository.GetAllData());
            var workexperiences = _mapper.Map<List<WorkexperienceDto>>(_workexperienceRepository.GetAllData());
            var professions = _mapper.Map<List<ProfessionDto>>(_professionRepository.GetAllData());
            var cities = _mapper.Map<List<CityDto>>(_cityRepository.GetAllData());
            var employers = _employersRepository.GetAllData();

            var query = from fv in f
                        join
                      j in job on fv.JobId equals j.JobId
                        join salary in salaries on j.SalaryId equals salary.SalaryId
                        join
                       formofwork in formofworks on j.FormofworkId equals formofwork.FormofworkId
                        join
                       levelwork in levelworks on j.LevelworkId equals levelwork.LevelworkId
                        join
                       workexperience in workexperiences on j.WorkexperienceId equals workexperience.WorkexperienceId
                        join
                       profession in professions on j.ProfessionId equals profession.ProfessionId
                        join
                       city in cities on j.CityId equals city.CityId
                        join employer in employers on j.EmployersId equals employer.EmployersId

                        where fv.IsFavoufite_Job==true
                        select new FavoutirJob
                        {
                            JobId = j.JobId,
                            JobName = j.JobName,
                            RequestJob = j.RequestJob,
                            BenefitsJob = j.BenefitsJob,
                            AddressJob = j.AddressJob,
                            WorkingTime = j.WorkingTime,
                            ExpirationDate = j.ExpirationDate,
                            WorkexperienceName = workexperience.WorkexperienceName,
                            FormofworkName = formofwork.FormofworkName,
                            CityName = city.CityName,
                            SalaryPrice = salary.SalaryPrice,
                            ProfessionName = profession.ProfessionName,
                            LevelworkName = levelwork.LevelworkName,
                            JobDescription = j.JobDescription,
                            CityId = city.CityId,
                            EmployersId=j.EmployersId,
                            Favoufite_Job_Id=fv.Favoufite_Job_Id,
                            FormofworkId = formofwork.FormofworkId,
                            IsFavoufite_Job=fv.IsFavoufite_Job,
                            Job_SeekerId= fv.Job_SeekerId,
                            LevelworkId= j.LevelworkId,
                            ProfessionId=j.ProfessionId,
                            SalaryId=j.SalaryId,
                            WorkexperienceId = j.WorkexperienceId,
                            CompanyLogo = employer.CompanyLogo,
                            CompanyName = employer.CompanyName,

                        };
            var paginatedResult = PaginatedList<FavoutirJob>.ToPageList(_mapper.Map<List<FavoutirJob>>(query), commonList.page, commonList.limit);
            return new PagedDataResponse<FavoutirJob>(paginatedResult, 200, query.Count());
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
