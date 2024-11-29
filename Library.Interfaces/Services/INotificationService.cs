using Library.Domain.Models;

namespace Library.Interfaces.Services;

public interface INotificationService
{
    Task SendNotificationAsync(NotificationModel notificationModel);
    Task<List<NotificationModel>> GetNotificationsForUserAsync(string userId);
    Task MarkAsReadAsync(string notificationId);

}