using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> GetAllSubjectsInfoAsync();
    Task<Subject?> GetSubjectByIdAsync(Guid id);
    Task AddSubjectAsync(Subject subject);
    Task UpdateSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(Guid id);
}