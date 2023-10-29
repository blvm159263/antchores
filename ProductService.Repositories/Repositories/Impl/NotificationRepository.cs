using ProductService.Repositories.Data;
using ProductService.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Repositories.Repositories.Impl
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Notification AddNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return notification;
        }

        public Notification DeleteNotification(int id)
        {
            _context.Notifications.Remove(GetNotification(id));
            _context.SaveChanges();

            return GetNotification(id);
        }

        public Notification GetNotification(int id)
        {
            return _context.Notifications.SingleOrDefault(n => n.Id == id);
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return _context.Notifications.ToList();
        }

        public Notification UpdateNotification(Notification notification)
        {
            _context.Entry(notification).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return notification;
        }
    }
}
