using Library.Domain;
using Library.Domain.DTOs.Users.Teacher;

namespace Library.Interfaces.Services;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
    Task<Result<TeacherDto, Error>> GetTeacherByIdAsync(Guid id);
    Task<Result<Ok, Error>> DeleteTeacherAsync(Guid id);
}