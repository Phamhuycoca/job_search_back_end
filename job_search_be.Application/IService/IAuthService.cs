using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface IAuthService
    {
        DataResponse<TokenDto> Login(LoginDto dto);
        DataResponse<TokenDto> Refresh_Token(RefreshTokenSettings token);
    }
}
