using AutoMapper;
using Library.Domain;
using Library.Domain.DTOs.Users.Student;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Library.Application;

public class StudentService(
    IStudentRepository studentRepository,
    IDepartmentRepository departmentRepository,
    IMapper mapper)
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

    public async Task<Result<IEnumerable<StudentDto>, Error>> GetStudentsByDepartmentIdAsync(int departmentId)
    {
        var department = await departmentRepository.FindDepartmentByIdAsync(departmentId);
        if (department == null)
            return new Error(StatusCodes.Status404NotFound, $"Not found department with id = {departmentId}");
        var students = await studentRepository.FindStudentsByDepartmentIdAsync(departmentId);
        return Result<IEnumerable<StudentDto>, Error>.Ok(mapper.Map<IEnumerable<StudentDto>>(students));
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