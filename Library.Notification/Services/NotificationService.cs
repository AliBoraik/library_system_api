using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Notification.Services;

public class NotificationService(INotificationRepository notificationRepository, IMapper mapper) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId)
    {
        var notifications = await notificationRepository.FindNotificationsByUserIdAsync(Guid.Parse(userId));
        return mapper.Map<IEnumerable<NotificationDto>>(notifications);
    }

    public async Task<Result<Ok, Error>> SendNotificationAsync(CreateNotificationDto createNotification)
    {
        // TODO check RecipientUserId  
        var notification = new NotificationModel
        {
            RecipientUserId = createNotification.RecipientUserId,
            Title = createNotification.Title,
            Message = createNotification.Message,
            SenderId = createNotification.SenderId,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
        await notificationRepository.AddNotificationAsync(notification);
        await notificationRepository.SaveChangesAsync();

        return new Ok();
    }

    public async Task<Result<Ok, Error>> MarkNotificationReadAsync(Guid notificationId)
    {
        await notificationRepository.MarkNotificationReadAsync(notificationId);
        await notificationRepository.SaveChangesAsync();
        return new Ok();
    }
}