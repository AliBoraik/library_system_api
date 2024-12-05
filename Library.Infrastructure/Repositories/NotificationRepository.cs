using Library.Domain.Models;
using Library.Domain.Models.MongoDbModels;
using Library.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Library.Infrastructure.Repositories;

public class NotificationRepository(IMongoDatabase database) : INotificationRepository
{
    private readonly IMongoCollection<NotificationModel> _notificationsCollection =
        database.GetCollection<NotificationModel>("Notifications");


    public async Task<List<NotificationModel>> FindUserNotificationAsync(Guid userId)
    {
        var notifications = await _notificationsCollection
            .Find(n => n.RecipientUserId == userId)
            .SortByDescending(n => n.SentAt)
            .ToListAsync();
        return notifications;
    }

    public async Task<ObjectId> AddNotificationAsync(NotificationModel notification)
    {
        await _notificationsCollection.InsertOneAsync(notification);
        return notification.Id;
    }

    public async Task<bool> MarkNotificationReadAsync(string notificationId)
    {
        var filter = Builders<NotificationModel>.Filter.Eq(n => n.Id, new ObjectId(notificationId));
        var update = Builders<NotificationModel>.Update.Set(n => n.IsRead, true);
        var result = await _notificationsCollection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }
}