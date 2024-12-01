using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> FindAllSubjectsInfoAsync();
    Task<Subject?> FindSubjectDetailsByIdAsync(Guid id);
    Task<Subject?> FindSubjectByIdAsync(Guid id);
    Task AddStudentToSubjectAsync(Guid studentId, Guid subjectId);
    Task<bool> SubjectExistsAsync(Guid id);
    Task AddSubjectAsync(Subject subject);
    Task UpdateSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(Subject id);
}