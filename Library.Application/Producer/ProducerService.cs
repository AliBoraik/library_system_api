using System.Text.Json;
using Confluent.Kafka;
using Library.Domain.DTOs.Notification;
using Library.Domain.Events.Notification;
using Library.Interfaces.Services;

namespace Library.Application.Producer;

public class ProducerService(ProducerConfig producerConfig) : IProducerService
{
    private readonly IProducer<Null, string> _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    public async Task SendNotificationEventAsync(string topic, NotificationEvent notification)
    {
        var message = JsonSerializer.Serialize(notification);
        await ProduceAsync(topic, message);
    }

    public async Task SendBulkNotificationEventToAsync(string topic, StudentBulkNotificationEvent bulkNotificationEvent)
    {
        var message = JsonSerializer.Serialize(bulkNotificationEvent);
        await ProduceAsync(topic, message);
    }

    public async Task ProduceAsync(string topic, string message)
    {
        var messagePack = new Message<Null, string> { Value = message };
        await _producer.ProduceAsync(topic, messagePack);
    }
}