using Library.Domain;
using Library.Domain.DTOs.Lecture;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces.Services;

public interface ILectureService
{
    Task<IEnumerable<LectureDto>> GetAllLecturesAsync();
    Task<Result<LectureDto, Error>> GetLectureByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddLectureAsync(CreateLectureDto lectureDto, IFormFile file);
    Task<Result<string, Error>> GetLectureFilePathByIdAsync(Guid id);
    Task<Result<Ok, Error>> UpdateLectureAsync(LectureDto lectureDto, IFormFile? file);
    Task<Result<Ok, Error>> DeleteLectureAsync(Guid id);
}