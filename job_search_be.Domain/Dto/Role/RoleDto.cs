using job_search_be.Domain.Dto.Permission;
using job_search_be.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Role
{
    public class RoleDto
    {
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public string? PermissionId { get; set; }
        public ICollection<string>? Permissions{ get; set; }
       
    }
}
