using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Infrastructure.Repositories;
using Library.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // For Identity and database
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var dockerEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_DOCKER");

            options.UseNpgsql(dockerEnv ?? configuration.GetConnectionString("LibraryDB"));
        });
        // For Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddTransient<ILectureRepository, LectureRepository>();
        services.AddTransient<ISubjectRepository, SubjectRepository>();
        services.AddTransient<IBookRepository, BookRepository>();
        return services;
    }
}