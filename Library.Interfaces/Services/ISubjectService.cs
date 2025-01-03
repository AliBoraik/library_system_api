using Library.Domain.DTOs.Subject;
using Library.Domain.Results;
using Library.Domain.Results.Common;

namespace Library.Interfaces.Services;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
    Task<Result<SubjectDetailsDto, Error>> GetUserSubjectByIdAsync(int subjectId, Guid userId);

    Task<Result<SubjectDetailsDto, Error>> GetSubjectByIdAsync(int subjectId);

    Task<Result<int, Error>> AddSubjectAsync(CreateSubjectDto subjectDto);

    Task<Result<Ok, Error>> UpdateSubjectAsync(SubjectDto subjectDto);
    Task<Result<Ok, Error>> DeleteSubjectAsync(int id);
}