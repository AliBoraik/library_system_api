using AutoMapper;
using Library.Domain.DTOs.Notification;
using Library.Domain.Events.Notification;
using Library.Domain.Models;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Notification.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    IStudentRepository studentRepository,
    IMapper mapper) : INotificationService
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

    public async Task<Result<Ok, Error>> SendBulkNotificationAsync(StudentBulkNotificationEvent bulkNotificationEvent)
    {
        var notificationModel = new NotificationModel
        {
            Title = bulkNotificationEvent.Title,
            Message = bulkNotificationEvent.Message,
            SentAt = DateTime.UtcNow,
            SenderUserId = bulkNotificationEvent.SenderId,
            UserNotifications = new List<UserNotification>()
        };
        var recipientUserIds =
            await studentRepository.FindStudentIdsByDepartmentIdAsync(bulkNotificationEvent.DepartmentId);
        foreach (var recipientUserId in recipientUserIds)
            notificationModel.UserNotifications.Add(new UserNotification
            {
                UserId = recipientUserId,
                IsRead = false
            });
        await notificationRepository.AddNotificationAsync(notificationModel);
        return ResultHelper.Ok();
    }

    public async Task<Result<Ok, Error>> SendNotificationAsync(NotificationEvent notificationEvent)
    {
        var notification = mapper.Map<NotificationModel>(notificationEvent);
        await notificationRepository.AddNotificationAsync(notification);
        return ResultHelper.Ok();
    }

    public async Task<Result<Ok, Error>> MarkNotificationReadAsync(Guid notificationId, Guid userId)
    {
        var userNotification = await notificationRepository.FindNotificationByIdAndUserIdAsync(notificationId, userId);
        if (userNotification == null)
            return Result<Ok, Error>.Err(Errors.NotFound("user notification"));

        userNotification.IsRead = true;
        await notificationRepository.SaveChangesAsync();
        return ResultHelper.Ok();
    }

    public async Task<Result<Ok, Error>> DeleteNotificationByIdAsync(Guid notificationId)
    {
        var notifications = await notificationRepository.FindNotificationByIdAsync(notificationId);
        if (notifications == null)
            return Result<Ok, Error>.Err(Errors.NotFound("notification"));
        await notificationRepository.DeleteNotificationAsync(notifications);
        return ResultHelper.Ok();
    }

    public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserIdAsync(Guid userId)
    {
        var notifications = await notificationRepository.FindUnreadNotificationsByUserIdAsync(userId);
        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }
}