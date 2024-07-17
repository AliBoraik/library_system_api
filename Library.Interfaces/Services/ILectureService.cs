using Library.Domain.DTOs;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services
{
    public interface ILectureService
    {
        Task<IEnumerable<LectureDto>> GetAllLecturesAsync();
        Task<LectureDto?> GetLectureByIdAsync(int id);
        Task AddLectureAsync(LectureDto lectureDto, IFormFile file);
        Task UpdateLectureAsync(LectureDto lectureDto, IFormFile? file);
        Task DeleteLectureAsync(int id);
    }
}