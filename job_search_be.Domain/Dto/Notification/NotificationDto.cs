using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Notification
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; }
        public Guid JobId { get; set; }
        public Guid Job_SeekerId { get; set; }
        public Guid EmployersId { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Notification_CreatedAt { get; set; }
    }
}
