using AutoMapper;
using Library.Domain.DTOs;
using Library.Domain.DTOs.Book;
using Library.Domain.DTOs.Department;
using Library.Domain.DTOs.Lecture;
using Library.Domain.DTOs.Subject;
using Library.Domain.Models;

namespace Library.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Department
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<Department, CreateDepartmentDto>().ReverseMap();
        CreateMap<Department, DepartmentDetailsDto>().ReverseMap();
        // Subject
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Subject, CreateSubjectDto>().ReverseMap();
        CreateMap<Subject, SubjectDetailsDto>().ReverseMap();
        // Lecture
        CreateMap<Lecture, LectureResponseDto>().ReverseMap();
        CreateMap<Lecture, CreateLectureDto>().ReverseMap();
        // Book
        CreateMap<Book, BookResponseDto>().ReverseMap();
        CreateMap<Book, CreateBookDto>().ReverseMap();
    }
}