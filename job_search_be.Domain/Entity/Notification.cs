using job_search_be.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Entity
{
    public class Notification:BaseEntity
    {
        public Guid NotificationId { get; set; }
        public Guid? JobId { get; set; }
        public Guid? Job_SeekerId { get; set; }
        public Guid? EmployersId { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Notification_CreatedAt { get; set; }
        public Job_Seeker? Job_Seeker { get; set; }
        public Employers? Employers { get; set; }
        public Job? Job { get; set; }
        public Notification() 
        {
            IsRead = false;
            createdAt = DateTime.Today.AddDays(1).AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
        }
    }
}
