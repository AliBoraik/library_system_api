using Library.Application.Configurations;
using Library.Auth.Services;
using Library.Infrastructure.Configurations;
using Library.Interfaces.Services;

namespace Library.Auth.Configurations;

public static class ConfigureServices
{
    public static void AddAuthApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthBuilder(configuration);
        // Redis OutputCache
        services.AddDatabaseConfiguration(configuration);
        // Add Redis connect for OutputCache
        services.AddRedisOutputCache(configuration);
        // register services 
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ITokenService, TokenService>();
        // add kafka ProducerConfig for sending notifications 
        services.AddKafkaProducerConfig(configuration);
        
    }
}