using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Users.Student;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class StudentService(IStudentRepository studentRepository, ISubjectRepository subjectRepository, IMapper mapper)
    : IStudentService
{
    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await studentRepository.FindStudentsAsync();
        return mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<Result<StudentDto, Error>> GetStudentByIdAsync(Guid id)
    {
        var student = await studentRepository.FindStudentByIdAsync(id);
        if (student == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found student with id = {id}");
        return mapper.Map<StudentDto>(student);
    }

    public async Task<Result<List<StudentDto>, Error>> GetStudentsBySubjectAsync(Guid subjectId)
    {
        var subject = await subjectRepository.SubjectExistsAsync(subjectId);
        if (!subject)
            return new Error(StatusCodes.Status404NotFound, $"Not found subject with id = {subjectId}");
        var students = await studentRepository.FindStudentsBySubjectAsync(subjectId);
        return mapper.Map<List<StudentDto>>(students);
    }


    public async Task<Result<Ok, Error>> DeleteStudentAsync(Guid id)
    {
        var studentExists = await studentRepository.FindStudentByIdAsync(id);
        if (studentExists == null)
            return new Error(StatusCodes.Status404NotFound, $"Can't found Student with ID = {id}");
        await studentRepository.DeleteStudentAsync(studentExists);
        return new Ok();
    }
}