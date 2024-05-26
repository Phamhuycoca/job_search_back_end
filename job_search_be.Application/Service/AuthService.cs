using AutoMapper;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserService _userService;
        public AuthService(IOptions<JWTSettings> jwtSettings, IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IRefreshTokenRepository refreshTokenRepository, IUserService userService)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _userService = userService;
        }

        public DataResponse<TokenDto> Login(LoginDto dto)
        {
            var user = _userRepository.GetAllData().Where(x => x.Email == dto.Email).SingleOrDefault();
            if (user == null)
            {
                throw new ApiException(401, "Tài khoản không tồn tại");
            }
            var isPasswordValid = PasswordHelper.VerifyPassword(dto.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new ApiException(401, "Mật khẩu không chính xác");
            }
            else
            {
                var roles = _roleRepository.GetAllData().Where(x => x.RoleId == user.RoleId);
                List<string> roleNames = new List<string>();
                foreach (var role in roles)
                {
                    roleNames.Add(role.RoleName);
                }
                user.Is_Active = true;
                _userRepository.Update(user);
                return new DataResponse<TokenDto>(CreateToken(user, roleNames), 200, "Đăng nhập thành công");
            }
            throw new ApiException(401, "Đăng nhập thất bại");
        }
      
       
        public DataResponse<TokenDto> Refresh_Token(RefreshTokenSettings token)
        {
            var existRefreshToken = _refreshTokenRepository.GetAllData().Where(x => x.RefreshToken == token.Refresh_Token).FirstOrDefault();
            if (existRefreshToken == null)
            {
                throw new ApiException(404, "Refresh Token không hợp lệ");
            }
            var user = _userRepository.GetById(existRefreshToken.UserId);
            if (user == null)
            {
                throw new ApiException(404, "Thông tin người dùng không tồn tại");
            }
            if (existRefreshToken.Refresh_TokenExpires < DateTime.Now)
            {
                throw new ApiException(404, "Refresh Token hết hạn");
            }
            var roles = _roleRepository.GetAllData().Where(x => x.RoleId == user.UserId);
            List<string> roleNames = new List<string>();
            foreach (var role in roles)
            {
                roleNames.Add(role.RoleName);
            }
            return new DataResponse<TokenDto>(RefreshToken(user, roleNames, _mapper.Map<RefreshTokenDto>(existRefreshToken)), 200, "Refresh token success");
        }












        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        public TokenDto CreateToken(User user, List<string> roles)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpiration);
            var securityKey = Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience[0],
                expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaims(user, _jwtSettings.Audience, roles),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = (int)((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
                RefreshTokenExpiration = (int)((DateTimeOffset)refreshTokenExpiration).ToUnixTimeSeconds()
            };
            var refresh_token = new RefreshTokenDto
            {
                UserId = user.UserId,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiration = tokenDto.RefreshTokenExpiration,
                Refresh_TokenExpires = refreshTokenExpiration
            };
            if (_refreshTokenRepository.GetById(user.UserId) == null)
            {
                _refreshTokenRepository.Create(_mapper.Map<Refresh_Token>(refresh_token));
            }
            else
            {
                _refreshTokenRepository.Update(_mapper.Map<Refresh_Token>(refresh_token));
            }
            return tokenDto;
        }
        private IEnumerable<Claim> GetClaims(User user, List<string> audiences, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            foreach (var permission in _userService.GetUserPermissions(user.UserId))
            {
                claims.Add(new Claim(CustomClaims.Permissions, permission.PermissionName));
            }
            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return claims;
        }

        public TokenDto RefreshToken(User user, List<string> roles, RefreshTokenDto refreshtoken)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpiration);
            var securityKey = Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience[0],
                expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaims(user, _jwtSettings.Audience, roles),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = (int)((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
                //RefreshTokenExpiration = (int)((DateTimeOffset)refreshTokenExpiration).ToUnixTimeSeconds()
                RefreshTokenExpiration = refreshtoken.RefreshTokenExpiration
            };
            var refresh_token = new RefreshTokenDto
            {
                UserId = user.UserId,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiration = refreshtoken.RefreshTokenExpiration,
                Refresh_TokenExpires = refreshtoken.Refresh_TokenExpires
            };
            _refreshTokenRepository.Update(_mapper.Map<Refresh_Token>(refresh_token));
            return tokenDto;
        }
    }
}
