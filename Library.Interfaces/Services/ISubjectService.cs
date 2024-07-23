using Library.Domain.DTOs;

namespace Library.Interfaces.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectInfoDto>> GetAllSubjectsAsync();
        Task<SubjectDto?> GetSubjectByIdAsync(Guid id);
        Task AddSubjectAsync(SubjectDto subjectDto);
        Task UpdateSubjectAsync(SubjectDto subjectDto);
        Task DeleteSubjectAsync(Guid id);
    }
}