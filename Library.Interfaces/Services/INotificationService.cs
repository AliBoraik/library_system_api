using Library.Domain;
using Library.Domain.DTOs.Notification;

namespace Library.Interfaces.Services;

public interface INotificationService
{
    Task<Result<Ok, Error>> SendNotificationAsync(CreateNotificationDto createNotification);
    Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId);
    Task<Result<Ok, Error>> MarkNotificationReadAsync(Guid notificationId);
}