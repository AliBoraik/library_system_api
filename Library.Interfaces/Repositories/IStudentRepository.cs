using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> FindStudentsAsync();
    Task<Student?> FindStudentByIdAsync(Guid id);
    Task<Student?> FindStudentByNameAsync(string name);
    Task<IEnumerable<Student>> FindStudentsByDepartmentIdAsync(int departmentId);
    Task<IEnumerable<Guid>> FindStudentIdsByDepartmentIdAsync(int departmentId);
    Task DeleteStudentAsync(Student student);
}