using Library.Domain.Events.Notification;

namespace Library.Interfaces.Services;

public interface IProducerService
{
    Task SendNotificationEventAsync(string topic, NotificationEvent notification);

    Task SendBulkNotificationEventToAsync(string topic, StudentBulkNotificationEvent bulkNotificationEvent);
    Task ProduceAsync(string topic, string message);
}