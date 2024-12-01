using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Library.Domain.Constants;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Notification;

namespace Library.Notification.Services;

[Authorize]
public class NotificationServiceImpl(INotificationRepository notificationRepository)
    : NotificationService.NotificationServiceBase
{
    public override async Task<SendNotificationResponse> SendNotification(SendNotificationRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.RecipientUserId, out var recipientId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, StringConstants.UserIdNotCorrect));
        var notification = new NotificationModel
        {
            RecipientUserId = recipientId,
            Title = request.Title,
            Message = request.Message,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
        var notificationId = await notificationRepository.AddNotificationAsync(notification);
        return new SendNotificationResponse { Success = true, NotificationId = notificationId.ToString() };
    }

    public override async Task<GetNotificationsResponse> GetNotifications(EmptyRequest request,
        ServerCallContext context)
    {
        // Get the current user's ID form token
        var userId = context.GetHttpContext().User.FindFirst(AppClaimTypes.Id)?.Value;
        if (userId == null)
            throw new RpcException(new Status(StatusCode.Unauthenticated, StringConstants.UserIdMissing));
        var notifications = await notificationRepository.FindUserNotificationAsync(Guid.Parse(userId));
        var response = new GetNotificationsResponse();
        response.Notifications.AddRange(notifications.Select(n => new NotificationDto
        {
            Id = n.Id.ToString(),
            Message = n.Message,
            Title = n.Title,
            SentAt = Timestamp.FromDateTime(n.SentAt.ToUniversalTime()),
            IsRead = n.IsRead
        }));
        return response;
    }

    public override async Task<MarkNotificationReadResponse> MarkNotificationRead(MarkNotificationReadRequest request,
        ServerCallContext context)
    {
        var result = await notificationRepository.MarkNotificationReadAsync(request.NotificationId);
        return new MarkNotificationReadResponse { Success = result };
    }
}