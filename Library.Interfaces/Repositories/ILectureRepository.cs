using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ILectureRepository
{
    Task<IEnumerable<Lecture>> GetAllLecturesAsync();
    Task<Lecture?> GetLectureByIdAsync(Guid id);
    Task<string> GetLectureFilePathByIdAsync(Guid id);
    Task AddLectureAsync(Lecture lecture);
    Task UpdateLectureAsync(Lecture lecture);
    Task DeleteLectureAsync(Guid id);
}