using Library.Application.CachePolicies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class RedisOutputConfig
{
    public static IServiceCollection AddRedisOutputCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis")!;
            options.InstanceName = "LibraryApp";
        });
        services.AddOutputCache(options =>
        {
            options.AddPolicy(nameof(AuthCachePolicy), AuthCachePolicy.Instance);
            options.DefaultExpirationTimeSpan = TimeSpan.FromDays(1);
        });

        /*services.AddOutputCache(options =>
        {
            options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
            options.AddBasePolicy(builder =>
            {
                builder.AddPolicy<AuthCachePolicy>();
            }, true);

        });*/

        return services;
    }
}