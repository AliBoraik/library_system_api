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
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ILectureRepository, LectureRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        return services;
    }
}