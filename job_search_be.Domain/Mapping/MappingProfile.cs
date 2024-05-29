using AutoMapper;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Favourite;
using job_search_be.Domain.Dto.FileCv;
using job_search_be.Domain.Dto.Formofwork;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Domain.Dto.Levelwork;
using job_search_be.Domain.Dto.News;
using job_search_be.Domain.Dto.Notification;
using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Dto.Profession;
using job_search_be.Domain.Dto.Recruitment;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.Salary;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Dto.Workexperience;
using job_search_be.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Permission, PermissionDto>().ReverseMap();
            CreateMap<Permission, PermissionQuery>().ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, RoleQuery>().ReverseMap();

            CreateMap<User, LoginDto>().ReverseMap();
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<Refresh_Token, RefreshTokenDto>().ReverseMap();

            CreateMap<User, UserQuery>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Formofwork, FormofworkDto>().ReverseMap();
            CreateMap<Formofwork, FormofworkQuery>().ReverseMap();

            CreateMap<Workexperience, WorkexperienceDto>().ReverseMap();
            CreateMap<Workexperience, WorkexperienceQuery>().ReverseMap();

            CreateMap<Salary, SalaryDto>().ReverseMap();
            CreateMap<Salary, SalaryQuery>().ReverseMap();

            CreateMap<Profession, ProfessionDto>().ReverseMap();
            CreateMap<Profession, ProfessionQuery>().ReverseMap();

            CreateMap<Job, JobDto>().ReverseMap();
            CreateMap<Job, JobQuery>().ReverseMap();
            CreateMap<Job, JobQueries>().ReverseMap();
            CreateMap<JobQueries, JobQueries>().ReverseMap();

            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityQuery>().ReverseMap();

            CreateMap<Employers, EmployersDto>().ReverseMap();
            CreateMap<Employers, EmployersUpdateDto>().ReverseMap();
            CreateMap<Employers, EmployersQuery>().ReverseMap();
            CreateMap<Employers,CompannyList>().ReverseMap();
            CreateMap<Employers,CompannyView>().ReverseMap();

            CreateMap<Employers, EmployersLogin>().ReverseMap();
            CreateMap<Employers_Refresh_Token, Employers_Refresh_TokenDto>().ReverseMap();


            CreateMap<Levelwork, LevelworkDto>().ReverseMap();
            CreateMap<Levelwork, LevelworkQuery>().ReverseMap();

            CreateMap<Job_Seeker, Job_Seeker_Register>().ReverseMap();
            CreateMap<Job_Seeker, Job_SeekerDto>().ReverseMap();
            CreateMap<Job_Seeker, Job_SeekerQuery>().ReverseMap();
            CreateMap<Job_Seeker, Job_SeekerUpdateDto>().ReverseMap();
            CreateMap<Job_Seeker, UploadCV>().ReverseMap();

            CreateMap<Job_Seeker, Job_Seeker_Login>().ReverseMap();
            CreateMap<Job_Seeker_Refresh_Token, Job_Seeker_Refresh_TokenDto>().ReverseMap();

            CreateMap<Recruitment, RecruitmentDto>().ReverseMap();
            CreateMap<Recruitment, RecruitmentQuery>().ReverseMap();
            CreateMap<Recruitment, RecruitmentList>().ReverseMap();
            CreateMap<Recruitment, RecruitmentChangeStatus>().ReverseMap();
            CreateMap<Recruitment, RecruitmentChangeFeedback>().ReverseMap();
            CreateMap<Recruitment, Job_Recruitment>().ReverseMap();

            CreateMap<FileCv,FileCvDto>().ReverseMap();
            CreateMap<FileCv,FileCvQuery>().ReverseMap();

            CreateMap<Favoufite_Job,FavouriteJobDto>().ReverseMap();

            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Notification, NotificationQuery>().ReverseMap();

            CreateMap<News, NewsDto>().ReverseMap();
            CreateMap<News, NewsQuery>().ReverseMap();

        }
    }
}
