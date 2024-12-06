using Library.Application.Configurations;
using Library.Infrastructure.Configurations;
using Library.Infrastructure.Repositories;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Library.Notification.Mapping;
using Library.Notification.Services;

namespace Library.Notification.Configurations;

public static class ConfigureServices
{
    public static void AddNotificationApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthBuilder(configuration);
        // Add Redis connect for OutputCache
        services.AddRedisOutputCache(configuration);
        // Add Database connect Configuration
        services.AddDatabaseConfiguration(configuration);
        // Add AutoMapper
        services.AddAutoMapper(typeof(NotificationAutoMapperProfile));

        // register services 
        services.AddScoped<INotificationService, NotificationService>();
        // register repository
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }
}