using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<NotificationModel>> FindNotificationsByUserIdAsync(Guid userId);
    Task<Guid> AddNotificationAsync(NotificationModel notification);
    Task MarkNotificationReadAsync(Guid notificationId);

    Task<NotificationModel?> FindNotificationByIdAsync(Guid notificationId);
    Task<IEnumerable<NotificationModel>> FindUnreadNotificationsByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
}