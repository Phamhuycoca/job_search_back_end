using AutoMapper;
using CloudinaryDotNet;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Auth;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Dashboard_Employers;
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
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TokenDto = job_search_be.Domain.Dto.Employers.TokenDto;

namespace job_search_be.Application.Service
{
    public class EmployersService : IEmployersService
    {
        private readonly IEmployersRepository _employersRepository;
        private readonly IMapper _mapper;
        private readonly IEmployers_Refresh_TokenRepository _refreshTokenRepository;
        private readonly JWTSettings _jwtSettings;
        private readonly Cloudinary _cloudinary;
        private readonly IJobRepository _jobRepository;
        private readonly IFormofworkRepository _formofworkRepository;
        private readonly ILevelworkRepository _levelworkRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ISalaryRepository _aryRepository;
        private readonly IWorkexperienceRepository _workexperienceRepository;
        private readonly IRecruitmentRepository _recruitmentRepository;

        public EmployersService(IEmployersRepository employersRepository,
            IMapper mapper, 
            IOptions<JWTSettings> jwtSettings,
            IEmployers_Refresh_TokenRepository refreshTokenRepository,
            Cloudinary cloudinary,
             IFormofworkRepository formofworkRepository,
            ILevelworkRepository levelworkRepository,
            ICityRepository cityRepository,
            IProfessionRepository professionRepository,
            ISalaryRepository aryRepository,
            IWorkexperienceRepository workexperienceRepository,
            IJobRepository jobRepository,
            IRecruitmentRepository recruitmentRepository)
        {
            _employersRepository = employersRepository;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _cloudinary = cloudinary;
            _formofworkRepository = formofworkRepository;
            _levelworkRepository = levelworkRepository;
            _cityRepository = cityRepository;
            _professionRepository = professionRepository;
            _aryRepository = aryRepository;
            _workexperienceRepository = workexperienceRepository;
            _employersRepository = employersRepository;
            _jobRepository = jobRepository;
            _recruitmentRepository = recruitmentRepository;
        }
        public DataResponse<EmployersQuery> Create(EmployersDto dto)
        {
            dto.EmployersId = Guid.NewGuid();
            dto.Password = PasswordHelper.CreateHashedPassword(dto.Password);
            var newData = _employersRepository.Create(_mapper.Map<Employers>(dto));
            if (newData != null)
            {
                return new DataResponse<EmployersQuery>(_mapper.Map<EmployersQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<EmployersQuery> Delete(Guid id)
        {
            var item = _employersRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            var data = _employersRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<EmployersQuery>(_mapper.Map<EmployersQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public DataResponse<EmployersQuery> GetById(Guid id)
        {
            var item = _employersRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            View(id);
            return new DataResponse<EmployersQuery>(_mapper.Map<EmployersQuery>(item), HttpStatusCode.OK, HttpStatusMessages.OK);
        }

        public PagedDataResponse<EmployersQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<EmployersQuery>>(_employersRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x => x.CompanyName.Contains(commonList.keyword)).ToList();
            }
            var paginatedResult = PaginatedList<EmployersQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<EmployersQuery>(paginatedResult, 200, query.Count());
        }

        public DataResponse<List<EmployersDto>> ItemsList()
        {
            var query = _employersRepository.GetAllData();
            if (query != null && query.Any())
            {
                var cityDtos = _mapper.Map<List<EmployersDto>>(query);
                return new DataResponse<List<EmployersDto>>(_mapper.Map<List<EmployersDto>>(query), HttpStatusCode.OK, HttpStatusMessages.OK);
            }
            throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
        }
      
        public DataResponse<EmployersQuery> Update(EmployersUpdateDto dto)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            var item = _employersRepository.GetById(dto.EmployersId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            if (dto.file != null)
            {
                if (item.CompanyLogo!= null)
                {
                    upload.DeleteImage(item.CompanyLogo);
                }
                dto.CompanyLogo = upload.ImageUpload(dto.file);
            }
            
            var newData = _employersRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<EmployersQuery>(_mapper.Map<EmployersQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }

        public DataResponse<TokenDto> Login(EmployersLogin dto)
        {
            var user = _employersRepository.GetAllData().Where(x => x.Email == dto.Email).SingleOrDefault();
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
                user.IsActive = true;
                _employersRepository.Update(user);
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

        public TokenDto CreateToken(Employers user)
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
            var refresh_token = new Employers_Refresh_TokenDto
            {
                EmployersId= user.EmployersId,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiration = tokenDto.RefreshTokenExpiration,
                Refresh_TokenExpires = refreshTokenExpiration
            };
            if (_refreshTokenRepository.GetById(user.EmployersId) == null)
            {
                _refreshTokenRepository.Create(_mapper.Map<Employers_Refresh_Token>(refresh_token));
            }
            else
            {
                _refreshTokenRepository.Update(_mapper.Map<Employers_Refresh_Token>(refresh_token));
            }
            return tokenDto;
        }
        private IEnumerable<Claim> GetClaims(Employers user, List<string> audiences)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.EmployersId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,user.Role)
            };
            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return claims;
        }

        public DataResponse<TokenDto> Refresh_Token(RefreshTokenSettings token)
        {
            var existRefreshToken = _refreshTokenRepository.GetAllData().Where(x => x.RefreshToken == token.Refresh_Token).FirstOrDefault();
            if (existRefreshToken == null)
            {
                throw new ApiException(404, "Refresh Token không hợp lệ");
            }
            var user = _employersRepository.GetById(existRefreshToken.EmployersId);
            if (user == null)
            {
                throw new ApiException(404, "Thông tin người dùng không tồn tại");
            }
            if (existRefreshToken.Refresh_TokenExpires < DateTime.Now)
            {
                throw new ApiException(404, "Refresh Token hết hạn");
            }
            
            return new DataResponse<TokenDto>(RefreshToken(user, _mapper.Map<Employers_Refresh_TokenDto>(existRefreshToken)), 200, "Refresh token success");
        }
        public TokenDto RefreshToken(Employers user, Employers_Refresh_TokenDto refreshtoken)
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
                Role=user.Role,
            };
            var refresh_token = new Employers_Refresh_TokenDto
            {
                EmployersId = user.EmployersId,
                RefreshToken = tokenDto.RefreshToken,
                RefreshTokenExpiration = refreshtoken.RefreshTokenExpiration,
                Refresh_TokenExpires = refreshtoken.Refresh_TokenExpires
            };
            _refreshTokenRepository.Update(_mapper.Map<Employers_Refresh_Token>(refresh_token));
            return tokenDto;
        }

        public PagedDataResponse<JobQueries> GetListJobById(CommonListQuery commonList,Guid id)
        {
            var query = _jobRepository.GetAllData();
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
                        join employer in employers on jobs.EmployersId equals employer.EmployersId where jobs.EmployersId.Equals(id)
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
                            CompanyName = employer.CompanyName
                        };
            var paginatedResult = PaginatedList<JobQueries>.ToPageList(items.ToList(), commonList.page, commonList.limit);
            return new PagedDataResponse<JobQueries>(paginatedResult, 200, items.Count());
        }

        public PagedDataResponse<CompannyList> CompannyList(CommonQueryByHome commonQueryByHome)
        {
            var employers = _employersRepository.GetAllData();
            var cities = _cityRepository.GetAllData();
            var items = from employer in employers
                        join
                      city in cities on employer.CityId equals city.CityId
                        select new CompannyList
                        {
                            CityId = employer.CityId,
                            CityName=city.CityName,
                            CompanyAddress = employer.CompanyAddress,
                            CompanyName = employer.CompanyName,
                            CompanyDescription = employer.CompanyDescription,
                            CompanyLogo= employer.CompanyLogo,
                            CompanyWebsite = employer.CompanyWebsite,
                            ContactEmail = employer.ContactEmail,
                            ContactPhoneNumber = employer.ContactPhoneNumber,
                            createdAt=employer.createdAt,
                            createdBy=employer.createdBy,
                            deletedAt=employer.deletedAt,
                            deletedBy=employer.deletedBy,
                            Email=employer.Email,
                            EmployersId=employer.EmployersId,
                            updatedAt=employer.updatedAt,
                            updatedBy=employer.updatedBy,

                        };

            if (!string.IsNullOrEmpty(commonQueryByHome.keyword))
            {
                items = items.Where(x =>
                                         x.CompanyName.Contains(commonQueryByHome.keyword) ||
                                         x.CompanyAddress.Contains(commonQueryByHome.keyword) ||
                                         x.CityName.Contains(commonQueryByHome.keyword) ||
                                         x.CompanyWebsite.Contains(commonQueryByHome.keyword) ||
                                         x.CompanyName.Contains(commonQueryByHome.keyword));
            }

            if (!string.IsNullOrEmpty(commonQueryByHome.cityId) && Guid.TryParse(commonQueryByHome.cityId, out var cityId))
            {
                items = items.Where(x => x.CityId.Equals(cityId));
            }
            var paginatedResult = PaginatedList<CompannyList>.ToPageList(items.ToList(), commonQueryByHome.page, commonQueryByHome.limit);
            return new PagedDataResponse<CompannyList>(paginatedResult, 200, items.Count());
        }

       /* public DataResponse<CompannyView> GetCompannyView(Guid id)
        {
            var item = _employersRepository.GetById(id);
            CompannyView result = new CompannyView();
            if(item.View == null)
            {
            result.View = 1;
            }
            else
            {
                result.View = item.View + 1;
            }
            var newData = _employersRepository.Update(_mapper.Map(result,item));
            if (newData != null)
            {
                return new DataResponse<CompannyView>(_mapper.Map<CompannyView>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }*/
        public void View(Guid id)
        {
            var item = _employersRepository.GetById(id);
            CompannyView result = new CompannyView();
            if (item.View == null)
            {
                result.View = 1;
            }
            else
            {
                result.View = item.View + 1;
            }
            var newData = _employersRepository.Update(_mapper.Map(result, item));
        }

        public DataResponse<Dashboard_Employers> Dashboard(Guid id)
        {
            var dash = new Dashboard_Employers()
            {
                TongSoBaiDang = _jobRepository.GetAllData().Where(x => x.EmployersId == id).Count(),
                TongSoCVChuaDuyet=_recruitmentRepository.GetAllData().Where(x=>x.EmployersId==id&&x.IsStatus==false).Count(),
                TongSoNguoiUngTuyen=_recruitmentRepository.GetAllData().Where(x=>x.EmployersId==id).Count(),
                TongSoNguoiDaGuiEmail= _recruitmentRepository.GetAllData().Where(x => x.EmployersId == id && x.IsStatus == true).Count()
            };
            return new DataResponse<Dashboard_Employers>(dash, 200, "Success");

        }

        public DataResponse<EmployerRegister> Register(EmployerRegister dto)
        {
            var check = _employersRepository.GetAllData().Where(x => x.Email == dto.Email).FirstOrDefault();
            if (check != null)
            {
                throw new ApiException(401, "Tài khoản đã tồn tại");
            }
            var dataObj = new Employers()
            {
                Email = dto.Email,
                Password = PasswordHelper.CreateHashedPassword(dto.Password),
                CompanyName = dto.CompanyName,
                Role= "Employer",
                CompanyLogo = "https://res.cloudinary.com/drhdgw1xx/image/upload/v1709880283/m76gmmyuzzv3phiyuy2u.jpg"
            };
            var newData = _employersRepository.Create(dataObj);
            if (newData != null)
            {
                return new DataResponse<EmployerRegister>(_mapper.Map<EmployerRegister>(newData), HttpStatusCode.OK, "Đăng ký tài khoản thành công, vui lòng đăng nhập.");
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, "Đăng ký thất bại");
        }
    }
}
 