using AutoMapper;
using job_search_be.Application.IService;
using job_search_be.Application.Service;
using job_search_be.Domain.Mapping;
using job_search_be.Infrastructure.Module;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Module
{
    public static class ApplicationModules
    {
        public static IServiceCollection AddApplicationModules(this IServiceCollection services)
        {
            var assm = Assembly.GetExecutingAssembly();
            services.AddInfrastructureModule();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFormofworkService, FormofworkService>();
            services.AddScoped<IWorkexperienceService, WorkexperienceService>();
            services.AddScoped<ISalaryService, SalaryService>();
            services.AddScoped<IProfessionService, ProfessionService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IEmployersService, EmployersService>();
            services.AddScoped<ILevelworkService, LevelworkService>();
            services.AddScoped<IJob_SeekerService, Job_SeekerService>();
            services.AddScoped<IRecruitmentService, RecruitmentService>();
            services.AddScoped<IFileCvService, FileCvService>();
            services.AddScoped<IFavouriteJobService, FavouriteJobService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<IEmailService, EmailService>();
            return services;
        }
    }
}
