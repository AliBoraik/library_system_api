using AutoMapper;
using Library.Domain.DTOs.Users.Student;
using Library.Domain.Results;
using Library.Domain.Results.Common;
using Library.Interfaces.Repositories;
using Library.Interfaces.Services;

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
            return Result<StudentDto, Error>.Err(Errors.NotFound("student"));
        var dto = mapper.Map<StudentDto>(student);
        return Result<StudentDto, Error>.Ok(dto);
    }

    public async Task<Result<IEnumerable<StudentDto>, Error>> GetStudentsByDepartmentIdAsync(int departmentId)
    {
        var department = await departmentRepository.FindDepartmentByIdAsync(departmentId);
        if (department == null)
            return Result<IEnumerable<StudentDto>, Error>.Err(Errors.NotFound("department"));
        var students = await studentRepository.FindStudentsByDepartmentIdAsync(departmentId);
        var dto = mapper.Map<IEnumerable<StudentDto>>(students);
        return Result<IEnumerable<StudentDto>, Error>.Ok(dto);
    }


    public async Task<Result<Ok, Error>> DeleteStudentAsync(Guid id)
    {
        var studentExists = await studentRepository.FindStudentByIdAsync(id);
        if (studentExists == null)
            return Result<Ok, Error>.Err(Errors.NotFound("student"));
        await studentRepository.DeleteStudentAsync(studentExists);
        return ResultHelper.Ok();
    }
}