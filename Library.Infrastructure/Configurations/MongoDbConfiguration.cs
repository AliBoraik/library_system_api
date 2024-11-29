using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Library.Infrastructure.Configurations;

public static class MongoDbConfiguration
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var dockerEnv = Environment.GetEnvironmentVariable("MongoDB");
        var mongoClient = new MongoClient(dockerEnv ?? configuration.GetConnectionString("MongoDB"));
        var database = mongoClient.GetDatabase("NotificationDB");
        services.AddSingleton(database);
        return services;
    }
}