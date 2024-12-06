using System.Text.Json;
using Confluent.Kafka;
using Library.Domain.DTOs.Notification;
using Library.Interfaces.Services;

namespace Library.Application;

public class ProducerService(ProducerConfig producerConfig) : IProducerService
{
    private readonly IProducer<Null, string> _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    
    /*
     
    // send notification
    var notificationRequest = new CreateNotificationDto
    {
        Message = $"Nwe department created with name {department.Name} and description {department.Description}",
        Title = "NEW DEPARTMENT CREATED",
        SenderId = Guid.Parse("fce4e50f-8a74-4747-9e43-f5f8e0684fd5"),
        RecipientUserId = Guid.Parse("fce4e50f-8a74-4747-9e43-f5f8e0684fd5")
    };

    // Run the producer service in the background
    _ = Task.Run(() =>
    producerService.SendNotificationEventAsync(AppTopics.NotificationTopic, notificationRequest));
    
    */


    public async Task SendNotificationEventAsync(string topic, CreateNotificationDto createNotification)
    {
        var message = JsonSerializer.Serialize(createNotification);
        await ProduceAsync(topic, message);
    }

    public async Task ProduceAsync(string topic, string message)
    {
        var messagePack = new Message<Null, string> { Value = message };
        await _producer.ProduceAsync(topic, messagePack);
    }
}