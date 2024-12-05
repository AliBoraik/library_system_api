using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Notification;
using Library.Domain.Models.MongoDbModels;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

namespace Library.Notification.Services;

public class NotificationService(INotificationRepository notificationRepository,  IMapper mapper) : INotificationService
{
    
    public async Task<Result<IEnumerable<NotificationDto>, Error>> GetNotificationsAsync(string userId)
    {
        var notifications = await notificationRepository.FindUserNotificationAsync(Guid.Parse(userId));
        return mapper.Map<List<NotificationDto>>(notifications);
    }
    
    public async Task<Result<SendNotificationResponse, Error>> SendNotificationAsync(SendNotificationRequest request , string userId)
    {
        var notification = new NotificationModel
        {
            RecipientUserId = request.RecipientUserId,
            Title = request.Title,
            Message = request.Message,
            SenderId = new Guid(userId),
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
        var notificationId = await notificationRepository.AddNotificationAsync(notification);
        
        return new SendNotificationResponse { Success = true, NotificationId = notificationId.ToString() };
    }

    public async Task<Result<Ok, Error>> MarkNotificationReadAsync(string notificationId)
    {
        var result = await notificationRepository.MarkNotificationReadAsync(notificationId);
        if (!result)
            return new Error(StatusCodes.Status400BadRequest ,  "Can not make notification read!");
        return new Ok();
    }
}