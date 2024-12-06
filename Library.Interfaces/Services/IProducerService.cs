using Library.Domain.DTOs.Notification;

namespace Library.Interfaces.Services;

public interface IProducerService
{
    Task SendNotificationEventAsync(string topic, CreateNotificationDto createNotification);
    Task ProduceAsync(string topic, string message);
}