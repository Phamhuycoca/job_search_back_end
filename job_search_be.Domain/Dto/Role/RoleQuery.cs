using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Role
{
    public class RoleQuery:BaseEntity
    {
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public string? PermissionId { get; set; }
        public string? PermissionName { get; set; }

    }
}
