using Library.Application.Mapping;
using Library.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class ConfigureServices
{
    public static void AddApiApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthBuilder(configuration);
        // Service 
        services.AddScoped<ILectureService, LectureService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IBookService, BookService>();
        services.AddSingleton<IUploadsService, UploadsService>();
        services.AddAutoMapper(typeof(ApiAutoMapperProfile));
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
    }
}