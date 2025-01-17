using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<UserNotification>> FindNotificationsByUserIdAsync(Guid userId);
    Task<IEnumerable<UserNotification>> FindLimitNotificationByUserIdAsync(Guid userId, int page, int limit);
    Task<Guid> AddNotificationAsync(NotificationModel notification);
    Task MarkNotificationReadAsync(Guid notificationId, Guid userId);

    Task<NotificationModel?> FindNotificationByIdAsync(Guid notificationId);

    Task<UserNotification?> FindNotificationByIdAndUserIdAsync(Guid notificationId, Guid userId);

    Task DeleteNotificationAsync(NotificationModel notificationId);
    Task<IEnumerable<UserNotification>> FindUnreadNotificationsByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
}