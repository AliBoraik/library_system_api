using Library.Domain;
using Library.Domain.DTOs.Notification;

namespace Library.Interfaces.Services;

public interface INotificationService
{
    Task<Result<SendNotificationResponse, Error>> SendNotificationAsync(NotificationRequest request);
    Task<Result<IEnumerable<NotificationDto>, Error>> GetNotificationsAsync(string userId);
    Task<Result<Ok, Error>> MarkNotificationReadAsync(string notificationId);
}