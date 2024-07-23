using Library.Domain.DTOs.Subject;

namespace Library.Interfaces.Services;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
    Task<SubjectDetailsDto> GetSubjectByIdAsync(Guid id);
    Task<Guid> AddSubjectAsync(CreateSubjectDto subjectDto);
    Task UpdateSubjectAsync(SubjectDto subjectDto);
    Task DeleteSubjectAsync(Guid id);
}