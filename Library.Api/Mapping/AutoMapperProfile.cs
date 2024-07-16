using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.Models;

namespace Library.Api.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<Department, AllDepartmentDto>().ReverseMap();
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Subject, SubjectInfoDto>();
        CreateMap<Lecture, LectureDto>().ReverseMap();
        CreateMap<Book, BookDto>().ReverseMap();
    }
    
}