using AutoMapper;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Notification.Services;

public class NotificationService(INotificationRepository notificationRepository, IMapper mapper) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync(Guid userId)
    {
        var notifications = await notificationRepository.FindNotificationsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }

    public async Task<IEnumerable<NotificationDto>> GetLimitNotificationsAsync(Guid userId, int page, int limit)
    {
        var notifications = await notificationRepository.FindLimitNotificationByUserIdAsync(userId, page, limit);
        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }

    public async Task<Result<Ok, Error>> SendBulkNotificationAsync(CreateBulkNotificationDto bulkNotificationDto)
    {
        var notificationModel = new NotificationModel
        {
            Title = bulkNotificationDto.Title,
            Message = bulkNotificationDto.Message,
            SentAt = DateTime.UtcNow,
            SenderUserId = bulkNotificationDto.SenderId,
            UserNotifications = new List<UserNotification>()
        };
        foreach (var recipientUserId in bulkNotificationDto.RecipientUserIds)
            notificationModel.UserNotifications.Add(new UserNotification
            {
                UserId = recipientUserId,
                IsRead = false
            });
        await notificationRepository.AddNotificationAsync(notificationModel);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> SendNotificationAsync(CreateNotificationDto createNotification)
    {
        // TODO check RecipientUserId  
        var notification = mapper.Map<NotificationModel>(createNotification);
        await notificationRepository.AddNotificationAsync(notification);

        return new Ok();
    }

    public async Task<Result<Ok, Error>> MarkNotificationReadAsync(Guid notificationId, Guid userId)
    {
        await notificationRepository.MarkNotificationReadAsync(notificationId, userId);
        return new Ok();
    }

    public async Task<Result<Ok, Error>> DeleteNotificationByIdAsync(Guid notificationId)
    {
        var notifications = await notificationRepository.FindNotificationByIdAsync(notificationId);
        if (notifications == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found with ID = {notificationId}");
        await notificationRepository.DeleteNotificationAsync(notifications);
        return new Ok();
    }

    public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserIdAsync(Guid userId)
    {
        var notifications = await notificationRepository.FindUnreadNotificationsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }
}