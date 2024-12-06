using System.Text.Json;
using Confluent.Kafka;
using Library.Domain.Constants;
using Library.Domain.DTOs.Notification;
using Library.Interfaces.Services;

namespace Library.Notification.Consumers;

public class NotificationSubscriberBackground(
    ConsumerConfig consumerConfig,
    ILogger<NotificationSubscriberBackground> logger,
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() => StartConsumer(stoppingToken), stoppingToken);
        return Task.CompletedTask;
    }

    private async Task StartConsumer(CancellationToken stoppingToken)
    {
        try
        {
            using var consumerBuilder = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumerBuilder.Subscribe(AppTopics.NotificationTopic);
            var cancelToken = new CancellationTokenSource();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // Create a scope using the CreateScope method
                    using var scope = scopeFactory.CreateScope();
                    // Resolve the user service using the GetRequiredService method
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    var consumer = consumerBuilder.Consume(cancelToken.Token);
                    var notificationRequest = JsonSerializer.Deserialize<CreateNotificationDto>(consumer.Message.Value);
                    if (notificationRequest != null)
                    {
                        await notificationService.SendNotificationAsync(notificationRequest);
                        logger.LogInformation("Processing notification title : {0}", notificationRequest.Title);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumerBuilder.Close();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during executing in NotificationSubscriberBackground");
            Console.WriteLine(ex.Message);
        }
    }
}