using Library.Application.Mapping;
using Library.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Configurations;

public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ILectureService, LectureService>();
        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<ISubjectService,SubjectService>();
        services.AddTransient<IBookService,BookService>();
        services.AddTransient<IUserService,UserService>();
        services.AddAutoMapper(typeof(AutoMapperProfile));
        return services;
    }
}