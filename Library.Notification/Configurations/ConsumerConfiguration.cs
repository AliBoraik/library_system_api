using Confluent.Kafka;

namespace Library.Notification.Configurations;

public  static class ConsumerConfiguration
{
    public static void AddConsumerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var consumerConfig = new ConsumerConfig
        {
            GroupId = "dummyGroup",
            BootstrapServers =   configuration.GetConnectionString("Kafka"),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        services.AddSingleton(consumerConfig);
    }
}