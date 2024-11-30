using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> FindStudentsAsync();
    Task<Student?> FindStudentByIdAsync(Guid id);
    Task<Student?> FindStudentByNameAsync(string name);
    Task<List<Student>> FindStudentsBySubjectAsync(Guid subjectId);
    Task DeleteStudentAsync(Student student);
}