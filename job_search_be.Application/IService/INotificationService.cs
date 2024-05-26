using job_search_be.Application.Helpers;
using job_search_be.Application.Wrappers.Concrete;
using job_search_be.Domain.Dto.City;
using job_search_be.Domain.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace job_search_be.Application.IService
{
    public interface INotificationService
    {
        PagedDataResponse<NotificationQuery> Items(CommonListQuery commonList,Guid id);
        DataResponse<List<NotificationDto>> ItemsList();
        DataResponse<NotificationQuery> Create(NotificationDto dto);
        DataResponse<NotificationQuery> Update(NotificationDto dto);
        DataResponse<NotificationQuery> Delete(Guid id);
        DataResponse<NotificationQuery> GetById(Guid id);
        DataResponse<List<NotificationQuery>> ItemsListByEmployer(Guid id);

    }
}
