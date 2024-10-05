using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> FindTeachersAsync();
    Task<Teacher?> FindTeacherByIdAsync(Guid id);

    Task<Teacher?> FindTeacherByNameAsync(string name);

    Task DeleteTeacherAsync(Teacher teacher);
}