using AutoMapper;
using CloudinaryDotNet;
using job_search_be.Application.Helpers;
using job_search_be.Application.IService;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Dto.Role;
using job_search_be.Domain.Dto.User;
using job_search_be.Domain.Entity;
using job_search_be.Domain.Repositories;
using job_search_be.Infrastructure.Exceptions;
using job_search_be.Infrastructure.Repositories;
using job_search_be.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace job_search_be.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly Cloudinary _cloudinary;
        public UserService(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IPermissionRepository permissionRepository,Cloudinary cloudinary)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _cloudinary = cloudinary;
        }
        public DataResponse<UserQuery> Create(UserDto dto)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            dto.UserId = Guid.NewGuid();
            dto.Password = PasswordHelper.CreateHashedPassword(dto.Password="12345678a");
            if (dto.file != null)
            {
                dto.Avatar=upload.ImageUpload(dto.file);
            }
            var newData = _userRepository.Create(_mapper.Map<User>(dto));
            if (newData != null)
            {
                return new DataResponse<UserQuery>(_mapper.Map<UserQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.AddedSuccesfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.AddedError);
        }

        public DataResponse<UserQuery> Delete(Guid id)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);

            var item = _userRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            if (item.Avatar != null)
            {
                upload.DeleteImage(item.Avatar);
            }
            var data = _userRepository.Delete(id);
            if (data != null)
            {
                return new DataResponse<UserQuery>(_mapper.Map<UserQuery>(item), HttpStatusCode.OK, HttpStatusMessages.DeletedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.DeletedError);
        }

        public PagedDataResponse<UserQuery> Items(CommonListQuery commonList)
        {
            var query = _mapper.Map<List<UserQuery>>(_userRepository.GetAllData());
            if (!string.IsNullOrEmpty(commonList.keyword))
            {
                query = query.Where(x =>
                    x.FullName.Contains(commonList.keyword) ||
                    x.Email.Contains(commonList.keyword)
                ).ToList();
            }
            foreach (var item in query)
            {
                    var permission = _roleRepository.GetById(item.RoleId ?? Guid.Empty);
                    if (permission != null)
                    {
                        item.RoleName= permission.RoleName;
                    }
            }
            var paginatedResult = PaginatedList<UserQuery>.ToPageList(query, commonList.page, commonList.limit);
            return new PagedDataResponse<UserQuery>(paginatedResult, 200, query.Count());

        }

        public DataResponse<UserQuery> Update(UserDto dto)
        {
            UpLoadImage upload = new UpLoadImage(_cloudinary);
            var item = _userRepository.GetById(dto.UserId);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            if (dto.imageDelete != null)
            {
                upload.DeleteImage(dto.imageDelete);
            }
            if (dto.file != null)
            {
                if(item.Avatar != null)
                {
                    upload.DeleteImage(item.Avatar);
                }
                dto.Avatar = upload.ImageUpload(dto.file);
            }
            var newData = _userRepository.Update(_mapper.Map(dto, item));
            if (newData != null)
            {
                return new DataResponse<UserQuery>(_mapper.Map<UserQuery>(newData), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);
            }
            throw new ApiException(HttpStatusCode.BAD_REQUEST, HttpStatusMessages.UpdatedError);
        }

        public DataResponse<UserQuery> GetById(Guid id)
        {
            var item = _userRepository.GetById(id);
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.ITEM_NOT_FOUND, HttpStatusMessages.NotFound);
            }
            return new DataResponse<UserQuery>(_mapper.Map<UserQuery>(item), HttpStatusCode.OK, HttpStatusMessages.UpdatedSuccessfully);

        }

        public List<Permission> GetUserPermissions(Guid id)
        {
            var user = _userRepository.GetById(id);
            var role = _mapper.Map<RoleDto>(_roleRepository.GetById(user.RoleId ?? Guid.Empty));
            string[] permissionIds = role.PermissionId.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            List<Permission> permissions = new List<Permission>();

            foreach (var permissionId in permissionIds)
            {
                var permission = _permissionRepository.GetByString(permissionId);

                if (permission != null)
                {
                    permissions.Add(permission);
                }
            }

            return permissions;

        }
    }
}
