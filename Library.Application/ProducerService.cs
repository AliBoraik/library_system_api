using System.Text.Json;
using Confluent.Kafka;
using Library.Domain.DTOs.Notification;
using Library.Interfaces.Services;

namespace Library.Application;

public class ProducerService(ProducerConfig producerConfig) : IProducerService
{
    private readonly IProducer<Null, string> _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    
    /*// send notification
    var notificationRequest = new NotificationRequest
    {
        Message = $"Nwe department created with name {department.Name} and description {department.Description}",
        Title = "NEW DEPARTMENT CREATED",
        SenderId = Guid.Parse("aba41058-354a-4c8d-9bbd-36f9258c272a"),
        RecipientUserId = Guid.Parse("aba41058-354a-4c8d-9bbd-36f9258c272a")
    };

    // Run the producer service in the background
    _ = Task.Run(() => producerService.SendNotificationEventAsync(AppTopics.NotificationTopic, notificationRequest));*/


    public async Task SendNotificationEventAsync(string topic, NotificationRequest notificationRequest)
    {
        var message = JsonSerializer.Serialize(notificationRequest);
        await ProduceAsync(topic, message);
    }

    public async Task ProduceAsync(string topic, string message)
    {
        var messagePack = new Message<Null, string> { Value = message };
        await _producer.ProduceAsync(topic, messagePack);
    }
}