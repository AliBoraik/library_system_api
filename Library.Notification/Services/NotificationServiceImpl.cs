using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Library.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Notification;

namespace Library.Notification.Services;

public class NotificationServiceImpl(IMongoDatabase database) : NotificationService.NotificationServiceBase
{
    private readonly IMongoCollection<NotificationModel> _notificationsCollection = database.GetCollection<NotificationModel>("Notifications");

    // Send Notification
      public override async Task<SendNotificationResponse> SendNotification(SendNotificationRequest request, ServerCallContext context)
    {
        var notification = new NotificationModel
        {
            RecipientUserId = request.RecipientUserId,
            Message = request.Message,
            SentAt = DateTime.UtcNow,
            IsRead = false,
        };

        await _notificationsCollection.InsertOneAsync(notification);

        return new SendNotificationResponse { Success = true, NotificationId = notification.Id.ToString() };
    }

    public override async Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request, ServerCallContext context)
    {
        var notifications = await _notificationsCollection
            .Find(n => n.RecipientUserId == request.UserId)
            .SortByDescending(n => n.SentAt)
            .ToListAsync();

        var response = new GetNotificationsResponse();
        response.Notifications.AddRange(notifications.Select(n => new NotificationDto
        {
            Id = n.Id.ToString(),
            Message = n.Message,
            SentAt = Timestamp.FromDateTime(n.SentAt.ToUniversalTime()),
            IsRead = n.IsRead
        }));

        return response;
    }

    public override async Task<MarkNotificationReadResponse> MarkNotificationRead(MarkNotificationReadRequest request, ServerCallContext context)
    {
        var filter = Builders<NotificationModel>.Filter.Eq(n => n.Id, new ObjectId(request.NotificationId));
        var update = Builders<NotificationModel>.Update.Set(n => n.IsRead, true);

        var result = await _notificationsCollection.UpdateOneAsync(filter, update);

        return new MarkNotificationReadResponse { Success = result.ModifiedCount > 0 };
    }
}