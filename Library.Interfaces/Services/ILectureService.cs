using Library.Domain.DTOs;
using Library.Domain.DTOs.Lecture;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services;

public interface ILectureService
{
    Task<IEnumerable<LectureDto>> GetAllLecturesAsync();
    Task<LectureDto?> GetLectureByIdAsync(Guid id);
    Task AddLectureAsync(CreateLectureDto lectureDto, IFormFile file);
    Task<string> GetLectureFilePathByIdAsync(Guid id);
    Task UpdateLectureAsync(LectureDto lectureDto, IFormFile? file);
    Task DeleteLectureAsync(Guid id);
}