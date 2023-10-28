using AutoMapper;
using ProductService.Repositories.Entities;
using ProductService.Repositories.Models;
using ProductService.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Services.Services.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public NotificationModel AddNotification(NotificationCreateModel notificationCreateModel)
        {
            var notification = _mapper.Map<Notification>(notificationCreateModel);

            notification = _notificationRepository.AddNotification(notification);

            var notificationModel = _mapper.Map<NotificationModel>(notification);

            return notificationModel;
        }

        public NotificationModel DeleteNotification(int id)
        {
            var notification = _notificationRepository.DeleteNotification(id);

            var notificationModel = _mapper.Map<NotificationModel>(notification);

            return notificationModel;
        }

        public NotificationModel GetNotification(int id)
        {
            var notification = _notificationRepository.GetNotification(id);

            var notificationModel = _mapper.Map<NotificationModel>(notification);

            return notificationModel;
        }

        public IEnumerable<NotificationModel> GetNotifications()
        {
            var notifications = _notificationRepository.GetNotifications();

            var notificationModels = _mapper.Map<IEnumerable<NotificationModel>>(notifications);

            return notificationModels;
        }

        public NotificationModel UpdateNotification(NotificationCreateModel notificationCreateModel, int id)
        {
            var notification = _mapper.Map<Notification>(notificationCreateModel);

            notification.Id = id;

            notification = _notificationRepository.UpdateNotification(notification);

            var notificationModel = _mapper.Map<NotificationModel>(notification);

            return notificationModel;
        }
    }
}
