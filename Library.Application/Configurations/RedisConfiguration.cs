using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class RedisConfiguration
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var dockerEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_REDIS");
        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.InstanceName = "Library-App";
            options.Configuration = configuration.GetConnectionString("Redis")!;
        });
        services.AddOutputCache(options =>
        {
            /*options.AddBasePolicy(builder =>
                builder.Expire(TimeSpan.FromSeconds(10)));*/
        });
        return services;
    }
}