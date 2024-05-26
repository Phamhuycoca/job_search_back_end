using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenDto = job_search_be.Domain.Dto.Job_Seeker.TokenDto;

namespace job_search_be.Application.IService
{
    public interface IJob_SeekerService
    {
        PagedDataResponse<Job_SeekerQuery> Items(CommonListQuery commonListQuery);
        DataResponse<Job_SeekerQuery> Create(Job_SeekerDto dto);
        DataResponse<Job_SeekerQuery> Update(Job_SeekerUpdateDto dto,string url);
        DataResponse<Job_SeekerQuery> Delete(Guid id);
        DataResponse<Job_SeekerQuery> GetById(Guid id);
        DataResponse<TokenDto> Login(Job_Seeker_Login dto);
        DataResponse<Job_Seeker_Register> Register(Job_Seeker_Register dto);
        DataResponse<TokenDto> Refresh_Token(RefreshTokenSettings token);
        DataResponse<TokenDto> Job_SeekerLoginByGoole(GoogleLoginRequest request);
        DataResponse<UploadCV> UploadCv(UploadCV uploadCV, string url);
    }
}
