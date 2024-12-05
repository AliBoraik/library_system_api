using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class KafkaProducerConfig
{
    public static void AddKafkaProducerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
        services.AddSingleton(producerConfig);
        services.AddSingleton<ProducerService>();
    }
}