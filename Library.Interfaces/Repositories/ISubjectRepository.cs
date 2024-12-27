using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> FindAllSubjectsInfoAsync();

    Task<Subject?> FindSubjectDetailsByIdAsync(int id);
    Task<Subject?> FindSubjectByIdAsync(int id);

    Task<bool> SubjectExistsAsync(int id);
    Task AddSubjectAsync(Subject subject);
    Task UpdateSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(Subject subject);
}