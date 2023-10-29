using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetNotifications();
        Notification GetNotification(int id);
        Notification AddNotification(Notification notification);
        Notification UpdateNotification(Notification notification);
        Notification DeleteNotification(int id);
    }
}
