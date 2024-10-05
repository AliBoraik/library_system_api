using Library.Domain;
using Library.Domain.DTOs.Lecture;

namespace Library.Interfaces.Services;

public interface ILectureService
{
    Task<IEnumerable<LectureResponseDto>> GetAllLecturesAsync();
    Task<Result<LectureResponseDto, Error>> GetLectureByIdAsync(Guid id);
    Task<Result<Guid, Error>> AddLectureAsync(CreateLectureDto lectureDto, Guid userId);
    Task<Result<Ok, Error>> DeleteLectureAsync(Guid lectureId, Guid userId);
    Task<Result<string, Error>> GetLectureFilePathByIdAsync(Guid id);
}