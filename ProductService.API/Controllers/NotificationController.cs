using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Repositories.Models;
using ProductService.Services.CacheService;
using ProductService.Services.Services;
using System.Collections.Generic;

namespace ProductService.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICacheService _cacheService;

        public NotificationController(INotificationService notificationService, ICacheService cacheService)
        {
            _notificationService = notificationService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var cacheKey = "notifications";

            var notifications = _cacheService.GetData<IEnumerable<NotificationModel>>(cacheKey);

            if (notifications == null)
            {
                notifications = _notificationService.GetNotifications();

                _cacheService.SetData(cacheKey, notifications);
            }

            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public IActionResult GetNotification(int id)
        {
            var cacheKey = $"notification-{id}";

            var notification = _cacheService.GetData<NotificationModel>(cacheKey);

            if (notification == null)
            {
                notification = _notificationService.GetNotification(id);

                _cacheService.SetData(cacheKey, notification);
            }

            return Ok(notification);
        }

        [HttpPost]
        public IActionResult AddNotification([FromBody] NotificationCreateModel notificationCreateModel)
        {
            var notification = _notificationService.AddNotification(notificationCreateModel);

            refreshCache(notification.Id);

            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotification(int id, [FromBody] NotificationCreateModel notificationCreateModel)
        {
            var notification = _notificationService.UpdateNotification(notificationCreateModel, id);

            refreshCache(id);

            return Ok(notification);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var notification = _notificationService.DeleteNotification(id);

            refreshCache(id);

            return Ok(notification);
        }

        private void refreshCache(int id)
        {
            _cacheService.RemoveData("notifications");
            _cacheService.RemoveData($"notification-{id}");
        }
    }
}
