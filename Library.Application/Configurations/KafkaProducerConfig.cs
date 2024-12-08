using Confluent.Kafka;
using Library.Application.Producer;
using Library.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class KafkaProducerConfig
{
    public static void AddKafkaProducerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration.GetConnectionString("Kafka")
        };
        services.AddSingleton(producerConfig);
        services.AddSingleton<IProducerService, ProducerService>();
    }
}