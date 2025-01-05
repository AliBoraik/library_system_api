using Library.Domain.DTOs.Notification;
using Library.Domain.Results;
using Library.Domain.Results.Common;

namespace Library.Interfaces.Services;

public interface INotificationService
{
    Task<Result<Ok, Error>> SendNotificationAsync(NotificationEvent notificationEvent);
    Task<Result<Ok, Error>> SendBulkNotificationAsync(StudentBulkNotificationEvent bulkNotificationEvent);
    Task<IEnumerable<NotificationDto>> GetNotificationsAsync(Guid userId);
    Task<IEnumerable<NotificationDto>> GetLimitNotificationsAsync(Guid userId, int page, int limit);
    Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserIdAsync(Guid userId);
    Task<Result<Ok, Error>> MarkNotificationReadAsync(Guid notificationId, Guid userId);

    Task<Result<Ok, Error>> DeleteNotificationByIdAsync(Guid notificationId);
}