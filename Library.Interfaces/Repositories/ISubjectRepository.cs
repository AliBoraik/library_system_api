using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ISubjectRepository
{
    Task<IEnumerable<Subject>> FindAllSubjectsInfoAsync();
    
    //Task<IEnumerable<Guid>> FindStudentIdsBySubjectAsync(int subjectId);
    Task<Subject?> FindSubjectDetailsByIdAsync(int id);
    Task<Subject?> FindSubjectByIdAsync(int id);
    
   // Task AddStudentToSubjectAsync(Guid studentId, int subjectId);
    Task<bool> SubjectExistsAsync(int id);
    Task AddSubjectAsync(Subject subject);
    Task UpdateSubjectAsync(Subject subject);
    Task DeleteSubjectAsync(Subject subject);
}