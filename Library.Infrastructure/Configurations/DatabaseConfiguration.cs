using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Configurations;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var dockerEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_DOCKER");

            options.UseNpgsql(dockerEnv ?? configuration.GetConnectionString("LibraryDB"));
        });
        // For Identity
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}