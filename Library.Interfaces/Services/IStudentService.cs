using Library.Domain;
using Library.Domain.DTOs.Users.Student;

namespace Library.Interfaces.Services;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<Result<StudentDto, Error>> GetStudentByIdAsync(Guid id);
    Task<Result<Ok, Error>> DeleteStudentAsync(Guid id);
}