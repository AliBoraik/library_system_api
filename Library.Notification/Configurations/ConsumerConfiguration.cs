using Confluent.Kafka;

namespace Library.Notification.Configurations;

public  static class ConsumerConfiguration
{
    public static void AddConsumerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var dockerEnv = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVERS");
        var consumerConfig = new ConsumerConfig
        {
            GroupId = "dummyGroup",
            BootstrapServers =   dockerEnv ??"localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        services.AddSingleton(consumerConfig);
    }
}