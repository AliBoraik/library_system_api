using Library.Infrastructure.DataContext;
using Library.Infrastructure.Repositories;
using Library.Interfaces;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var dockerEnv = Environment.GetEnvironmentVariable("CONNECTION_STRING_DOCKER");
            
            options.UseNpgsql(dockerEnv ?? configuration.GetConnectionString("LibraryDB"));
        });
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddTransient<ILectureRepository, LectureRepository>();
        services.AddTransient<ISubjectRepository, SubjectRepository>();
        services.AddTransient<IBookRepository, BookRepository>();
        return services;
    }
}