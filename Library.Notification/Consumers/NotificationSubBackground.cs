using System.Text.Json;
using Confluent.Kafka;
using Library.Domain.Constants;
using Library.Domain.DTOs.Notification;
using Library.Domain.Events.Notification;
using Library.Interfaces.Services;

namespace Library.Notification.Consumers;

public class NotificationSubBackground(
    ConsumerConfig consumerConfig,
    ILogger<NotificationSubBackground> logger,
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumerBuilder =
        new ConsumerBuilder<Ignore, string>(consumerConfig).Build();


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        try
        {
            _consumerBuilder.Subscribe(AppTopics.NotificationTopic);
            logger.LogInformation("Kafka consumer has subscribed");
            while (!stoppingToken.IsCancellationRequested)
            {
                // Create a scope using the CreateScope method
                using var scope = scopeFactory.CreateScope();
                // Resolve the user service using the GetRequiredService method
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var consumeResult = _consumerBuilder.Consume(stoppingToken);

                // Deserialize the message
                var message = consumeResult.Message.Value;

                if (message.Contains("DepartmentId")) // Identify Bulk Notification
                {
                    var bulkNotification = JsonSerializer.Deserialize<StudentBulkNotificationEvent>(message);
                    if (bulkNotification != null)
                    {
                        var result = await notificationService.SendBulkNotificationAsync(bulkNotification);
                        if (result.IsOk)
                            logger.LogInformation("Kafka Consumer consumed new bulk notifications !");
                    }
                }
                else // Single Notification
                {
                    var notification = JsonSerializer.Deserialize<NotificationEvent>(message);
                    if (notification != null)
                    {
                        var result = await notificationService.SendNotificationAsync(notification);
                        if (result.IsOk)
                            logger.LogInformation("Kafka Consumer consumed new notification !");
                    }
                }
            }
        }
        catch (ConsumeException ce)
        {
            logger.LogError($"Consume error: {ce.Error.Reason}");
        }
        finally
        {
            logger.LogInformation("The Kafka consumer thread has been cancelled");
            _consumerBuilder.Close();
        }
    }

    public override void Dispose()
    {
        _consumerBuilder.Dispose();
        base.Dispose();
    }
}