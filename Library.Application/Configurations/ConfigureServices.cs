using Library.Application.Mapping;
using Library.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILectureService, LectureService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IUserService, UserService>();
        services.AddAutoMapper(typeof(AutoMapperProfile));
        return services;
    }
}