using Library.Domain;
using Library.Domain.DTOs.Notification;

namespace Library.Interfaces.Services;

public interface INotificationService
{
    Task<Result<Ok, Error>> SendNotificationAsync(CreateNotificationDto createNotification);
    Task<Result<Ok, Error>> SendBulkNotificationAsync(CreateBulkNotificationDto bulkNotificationDto);
    Task<IEnumerable<NotificationDto>> GetNotificationsAsync(Guid userId);
    Task<IEnumerable<NotificationDto>> GetLimitNotificationsAsync(Guid userId , int page, int limit);
    Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserIdAsync(Guid userId);
    Task<Result<Ok, Error>> MarkNotificationReadAsync(Guid notificationId , Guid userId);
    
    Task<Result<Ok, Error>> DeleteNotificationByIdAsync(Guid notificationId);
    
}