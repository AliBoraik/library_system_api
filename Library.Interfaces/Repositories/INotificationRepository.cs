using Library.Domain.Models;
using MongoDB.Bson;

namespace Library.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<List<NotificationModel>> FindUserNotificationAsync(Guid userId);
    Task<ObjectId> AddNotificationAsync(NotificationModel notification);
    Task<bool> MarkNotificationReadAsync(string notificationId);
}