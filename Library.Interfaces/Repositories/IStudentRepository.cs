using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> FindStudentsAsync();
    Task<Student?> FindStudentByIdAsync(Guid id);
    Task<Student?> FindStudentByNameAsync(string name);
    Task DeleteStudentAsync(Student student);
}