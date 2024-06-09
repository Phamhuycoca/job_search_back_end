using AutoMapper;
using CloudinaryDotNet;
using Google.Apis.Auth;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Employers;
using job_search_be.Domain.Dto.Job;
using job_search_be.Domain.Dto.Job_Seeker;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Repositories;
using job_search_be.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TokenDto = job_search_be.Domain.Dto.Job_Seeker.TokenDto;

namespace job_search_be.Application.Service
{
    public class Job_SeekerService:IJob_SeekerService
    {
        private readonly IJob_SeekerRepository _job_SeekerRepository;
        private readonly IMapper _mapper;
        private readonly JWTSettings _jwtSettings;
        private readonly Cloudinary _cloudinary;
        private readonly IJob_Seeker_Refresh_Token_Repository _job_SeekerRefreshTokenRepository;
        public Job_SeekerService(IOptions<JWTSettings> jwtSettings,IJob_SeekerRepository job_SeekerRepository, IMapper mapper, IJob_Seeker_Refresh_Token_Repository job_SeekerRefreshTokenRepository,Cloudinary cloudinary)
        {
            _job_SeekerRepository = job_SeekerRepository;
            _mapper = mapper;
            _job_SeekerRefreshTokenRepository = job_SeekerRefreshTokenRepository;
            _cloudinary = cloudinary;
            _jwtSettings = jwtSettings.Value;
        }

        public DataResponse<Job_SeekerQuery> Create(Job_SeekerDto dto)
        {
            dto.Job_SeekerId= Guid.NewGuid();
            dto.Password = PasswordHelper.CreateHashedPassword(dto.Password);
            var newData = _job_SeekerRepository.Create(_mapper.Map<Job_Seeker>(dto));
            if (newData != null)
            {
                return new DataResponse<Job_SeekerQuery>(_mapper.Map<Job_SeekerQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<Job_SeekerQuery> Delete(Guid id)
        {
            var item = _job_SeekerRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _job_SeekerRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<Job_SeekerQuery>(_mapper.Map<Job_SeekerQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<Job_SeekerQuery> GetById(Guid id)
        {
            var item = _job_SeekerRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<Job_SeekerQuery>(_mapper.Map<Job_SeekerQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<Job_SeekerQuery> Items(CommonListQuery commonListQuery)
        {
            var query = _mapper.Map<List<Job_SeekerQuery>>(_job_SeekerRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonListQuery.keyword))
            {
                query = query.Where(x => x.FullName.Contains(commonListQuery.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<Job_SeekerQuery>.ToPageList(query, commonListQuery.page, commonListQuery.limit);
            return new PagedDataResponse<Job_SeekerQuery>(paginatedResult, 200, query.Count());
        }       

        public DataResponse<Job_SeekerQuery> Update(Job_SeekerUpdateDto dto,string url)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            var item = _job_SeekerRepository.GetById(dto.Job_SeekerId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            if (dto.cv != null)
            {
                /*if(item.Job_Cv != null ||item.Job_Cv=="string")
                {
                    string[] path = item.Job_Cv.Split(url);
                    if (!string.IsNullOrEmpty(path[1]))
                    {
                        FileUploadService.DeletePDF(path[1]);
                    }
                    FileUploadService.DeletePDF(item.Job_Cv);
                }*/
                dto.Job_Cv =url+ FileUploadService.CreatePDF(dto.cv);
            }
            if(dto.avt != null )
            {
                if (item.Avatar != null || item.Avatar == "string")
                {
                    upload.DeleteImage(item.Avatar);
                }
                dto.Avatar=upload.ImageUpload(dto.avt);
            }
            var newData = _job_SeekerRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<Job_SeekerQuery>(_mapper.Map<Job_SeekerQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }










        public DataResponse<TokenDto> Login(Job_Seeker_Login dto)
        {
            var user = _job_SeekerRepository.GetAllData().Where(x => x.Email == dto.Email).FirstOrDefault();
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
                _job_SeekerRepository.Update(user);
                return new DataResponse<TokenDto>(CreateToken(user), 200, "Đăng nhập thành công");
            }
            throw new ApiException(401, "Đăng nhập thất bại");
        }
        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }

        public TokenDto CreateToken(Job_Seeker user)
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
                 claims: GetClaims(user, _jwtSettings.Audience),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = (int)((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
                RefreshTokenExpiration = (int)((DateTimeOffset)refreshTokenExpiration).ToUnixTimeSeconds(),
                Role=user.Role
            };
            var refresh_token = new Job_Seeker_Refresh_TokenDto
            {
                Job_SeekerId = user.Job_SeekerId,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiration = tokenDto.RefreshTokenExpiration,
                Refresh_TokenExpires = refreshTokenExpiration
            };
            if (_job_SeekerRefreshTokenRepository.GetById(user.Job_SeekerId) == null)
            {
                _job_SeekerRefreshTokenRepository.Create(_mapper.Map<Job_Seeker_Refresh_Token>(refresh_token));
            }
            else
            {
                _job_SeekerRefreshTokenRepository.Update(_mapper.Map<Job_Seeker_Refresh_Token>(refresh_token));
            }
            return tokenDto;
        }
        private IEnumerable<Claim> GetClaims(Job_Seeker user, List<string> audiences)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Job_SeekerId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,user.Role)
            };
            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return claims;
        }
        public DataResponse<TokenDto> Refresh_Token(RefreshTokenSettings token)
        {
            var existRefreshToken = _job_SeekerRefreshTokenRepository.GetAllData().Where(x => x.RefreshToken == token.Refresh_Token).FirstOrDefault();
            if (existRefreshToken == null)
            {
                throw new ApiException(404, "Refresh Token không hợp lệ");
            }
            var user = _job_SeekerRepository.GetById(existRefreshToken.Job_SeekerId);
            if (user == null)
            {
                throw new ApiException(404, "Thông tin người dùng không tồn tại");
            }
            if (existRefreshToken.Refresh_TokenExpires < DateTime.Now)
            {
                throw new ApiException(404, "Refresh Token hết hạn");
            }

            return new DataResponse<TokenDto>(RefreshToken(user, _mapper.Map<Job_Seeker_Refresh_TokenDto>(existRefreshToken)), 200, "Refresh token success");
        }
        public TokenDto RefreshToken(Job_Seeker user, Job_Seeker_Refresh_TokenDto refreshtoken)
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
                 claims: GetClaims(user, _jwtSettings.Audience),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = (int)((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
                RefreshTokenExpiration = refreshtoken.RefreshTokenExpiration,
                Role= user.Role
            };
            var refresh_token = new Job_Seeker_Refresh_TokenDto
            {
                Job_SeekerId = user.Job_SeekerId,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiration = refreshtoken.RefreshTokenExpiration,
                Refresh_TokenExpires = refreshtoken.Refresh_TokenExpires
            };
            _job_SeekerRefreshTokenRepository.Update(_mapper.Map<Job_Seeker_Refresh_Token>(refresh_token));
            return tokenDto;
        }

        public DataResponse<TokenDto> Job_SeekerLoginByGoole(GoogleLoginRequest request)
        {
            try
            {
                var validPayload = GoogleJsonWebSignature.ValidateAsync(request.Credential).Result;
                if (validPayload == null)
                {
                    throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.TokenOrUserNotFound);
                }
                if (!checkEmail(validPayload.Email))
                {

                    var job_seeker = _job_SeekerRepository.GetAllData().Where(x => x.Email == validPayload.Email).SingleOrDefault();
                    _job_SeekerRepository.Update(job_seeker);
                    var token = CreateToken(job_seeker);
                    var tokenres = new TokenDto()
                    {
                        AccessToken = token.AccessToken,
                        AccessTokenExpiration = token.AccessTokenExpiration,
                        RefreshToken=token.RefreshToken,
                        RefreshTokenExpiration=token.RefreshTokenExpiration,
                        Role = token.Role

                    };
                    return new DataResponse<TokenDto>(tokenres, 200, "Đăng nhập thành công");
                }
                else
                {

                    var dto = new Job_SeekerDto()
                    {
                        Job_SeekerId = Guid.NewGuid(),
                        Email = validPayload.Email,
                        Avatar = validPayload.Picture,
                        FullName = validPayload.Name,
                        Job_Cv = null
                    };
                    var user = _job_SeekerRepository.Create(_mapper.Map<Job_Seeker>(dto));
                    var token = CreateToken(_mapper.Map<Job_Seeker>(user));
                    var tokenres = new TokenDto()
                    {
                        AccessToken = token.AccessToken,
                        AccessTokenExpiration = token.AccessTokenExpiration,
                        RefreshToken = token.RefreshToken,
                        RefreshTokenExpiration = token.RefreshTokenExpiration,
                        Role = token.Role
                    };
                    return new DataResponse<TokenDto>(tokenres, 200, "Đăng nhập thành công");
                }
            }
            catch (InvalidJwtException)
            {
                throw new ApiException(HttpStatusCode.BAD_REQUEST, "Token không hợp lệ");
            }
        }
        public bool checkEmail(string email)
        {
            var job_seeker = _job_SeekerRepository.GetAllData().FirstOrDefault(x => x.Email.Equals(email));
            if (job_seeker == null)
            {
                return true;
            }
            return false;
        }

        public DataResponse<UploadCV> UploadCv(UploadCV uploadCV, string url)
        {
            var item = _job_SeekerRepository.GetById(uploadCV.Job_SeekerId);
            if(uploadCV.DeleteCv != null) 
            {
                string[] path = uploadCV.DeleteCv.Split(url);
                if (!string.IsNullOrEmpty(path[1]))
                {
                    FileUploadService.DeletePDF(path[1]);
                }
                uploadCV.Job_Cv = "";
            }
            if (uploadCV.cv != null)
            {
                if (item.Job_Cv != null || item.Job_Cv == "string")
                {
                    string[] path = item.Job_Cv.Split(url);
                    if (!string.IsNullOrEmpty(path[1]))
                    {
                        FileUploadService.DeletePDF(path[1]);
                    }
                    FileUploadService.DeletePDF(item.Job_Cv);
                }
                uploadCV.Job_Cv = url + FileUploadService.CreatePDF(uploadCV.cv);
            }
            var newData = _job_SeekerRepository.Update(_mapper.Map(uploadCV, item));
            return new DataResponse<UploadCV>(_mapper.Map<UploadCV>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);

        }

        public DataResponse<Job_Seeker_Register> Register(Job_Seeker_Register dto)
        {
            var check=_job_SeekerRepository.GetAllData().Where(x=>x.Email==dto.Email).FirstOrDefault();
            if(check != null)
            {
                throw new ApiException(401, "Tài khoản đã tồn tại");
            }
            var dataObj = new Job_Seeker()
            {
                Email = dto.Email,
                Password = PasswordHelper.CreateHashedPassword(dto.Password),
                FullName=dto.FullName,
                Role= "Job_seeker",
                Avatar= "https://res.cloudinary.com/drhdgw1xx/image/upload/v1709880283/m76gmmyuzzv3phiyuy2u.jpg"
            };
            var newData = _job_SeekerRepository.Create(dataObj);
            if (newData != null)
            {
                return new DataResponse<Job_Seeker_Register>(_mapper.Map<Job_Seeker_Register>(newData), HttpStatusCode.OK, "Đăng ký tài khoản thành công, vui lòng đăng nhập.");
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, "Đăng ký thất bại");
        }
    }
}
