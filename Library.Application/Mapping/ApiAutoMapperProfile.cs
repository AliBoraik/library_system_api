using AutoMapper;
using Library.Domain.DTOs.Book;
using Library.Domain.DTOs.Department;
using Library.Domain.DTOs.Lecture;
using Library.Domain.DTOs.Subject;
using Library.Domain.DTOs.Users.Student;
using Library.Domain.DTOs.Users.Teacher;
using Library.Domain.Models;

namespace Library.Application.Mapping;

public class ApiAutoMapperProfile : Profile
{
    public ApiAutoMapperProfile()
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
        // Student
        CreateMap<User, StudentDto>().ReverseMap();
        CreateMap<User, CreateStudent>().ReverseMap();
        CreateMap<Student, StudentDto>()
            .IncludeMembers(s => s.User);
        CreateMap<Student, CreateStudent>()
            .IncludeMembers(s => s.User);
        // Teacher
        CreateMap<User, TeacherDto>().ReverseMap();
        CreateMap<User, CreateTeacher>().ReverseMap();
        CreateMap<Teacher, TeacherDto>()
            .IncludeMembers(s => s.User)
            .ReverseMap();
        CreateMap<Teacher, CreateTeacher>()
            .IncludeMembers(s => s.User)
            .ReverseMap();
    }
}