using Library.Domain;
using Library.Domain.DTOs.Subject;

namespace Library.Interfaces.Services;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
    Task<Result<SubjectDetailsDto, Error>> GetSubjectByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddSubjectAsync(CreateSubjectDto subjectDto);
    Task<Result<Ok, Error>> AddStudentToSubjectAsync(Guid studentId, Guid subjectId);

    Task<Result<Ok, Error>> UpdateSubjectAsync(SubjectDto subjectDto);
    Task<Result<Ok, Error>> DeleteSubjectAsync(Guid id);
}