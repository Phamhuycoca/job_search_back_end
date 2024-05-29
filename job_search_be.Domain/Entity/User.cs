using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class User:BaseEntity
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public Guid? RoleId { get; set; }
        public bool? Is_Active { get; set; }
        public Role? Role { get; set; }
        public virtual ICollection<Refresh_Token>? Refresh_Tokens { get; set; }
        public virtual ICollection<News>? News { get; set; }
    }
}
