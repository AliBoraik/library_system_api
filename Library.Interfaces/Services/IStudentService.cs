using Library.Domain.DTOs.Users.Student;
using Library.Domain.Results;
using Library.Domain.Results.Common;

namespace Library.Interfaces.Services;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<Result<StudentDto, Error>> GetStudentByIdAsync(Guid id);
    Task<Result<IEnumerable<StudentDto>, Error>> GetStudentsByDepartmentIdAsync(int departmentId);

    Task<Result<Ok, Error>> DeleteStudentAsync(Guid id);
}