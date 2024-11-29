using Library.Infrastructure.Repositories;
using Library.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ILectureRepository, LectureRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        return services;
    }
}