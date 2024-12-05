using Library.Application.Configurations;
using Library.Infrastructure.Configurations;
using Library.Interfaces.Services;
using AuthService = Library.Auth.Services.AuthService;
using TokenService = Library.Auth.Services.TokenService;

namespace Library.Auth.Configurations;

public static class ConfigureServices
{
    public static void AddAuthApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthBuilder(configuration);
        // Redis OutputCache
        services.AddRedisOutputCache(configuration);
        // Add Database connect Configuration
        services.AddDatabaseConfiguration(configuration); 
        // Add Redis connect for OutputCache
        services.AddRedisOutputCache(configuration);
        
        // register services 
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ITokenService , TokenService>();
    }
}