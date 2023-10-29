using ProductService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services
{
    public interface INotificationService
    {
        IEnumerable<NotificationModel> GetNotifications();
        NotificationModel GetNotification(int id);
        NotificationModel AddNotification(NotificationCreateModel notificationCreateModel);
        NotificationModel UpdateNotification(NotificationCreateModel notificationCreateModel, int id);
        NotificationModel DeleteNotification(int id);
    }
}
