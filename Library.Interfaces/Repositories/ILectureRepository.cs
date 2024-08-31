using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ILectureRepository
{
    Task<IEnumerable<Lecture>> FindAllLecturesAsync();
    Task<Lecture?> FindLectureByIdAsync(Guid id);
    Task<Lecture?> FindLectureByNameAsync(string name , Guid subjectId);
    Task<string?> FindLectureFilePathByIdAsync(Guid id);
    Task AddLectureAsync(Lecture lecture);
    Task DeleteLectureAsync(Lecture id);
}