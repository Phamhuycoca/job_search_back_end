using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Domain.Dto.Favourite
{
    public class FavouriteJobDto
    {
        public Guid? Favoufite_Job_Id { get; set; }
        public Guid? JobId { get; set; }
        public bool? IsFavoufite_Job { get; set; }
        public Guid? Job_SeekerId { get; set; }
    }
}
