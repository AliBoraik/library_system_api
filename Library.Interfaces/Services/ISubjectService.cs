using Library.Domain.DTOs;

namespace Library.Interfaces.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectInfoDto>> GetAllSubjectsAsync();
        Task<SubjectDto?> GetSubjectByIdAsync(int id);
        Task AddSubjectAsync(SubjectDto subjectDto);
        Task UpdateSubjectAsync(SubjectDto subjectDto);
        Task DeleteSubjectAsync(int id);
    }
}