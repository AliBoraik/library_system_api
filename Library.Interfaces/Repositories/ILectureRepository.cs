using Library.Domain.Models;

namespace Library.Interfaces.Repositories;

public interface ILectureRepository
{
    Task<IEnumerable<Lecture>> FindAllLecturesAsync();
    Task<Lecture?> FindLectureWithSubjectByIdAsync(Guid id);
    Task<Lecture?> FindLectureByNameAsync(string name, int subjectId);
    Task<string?> FindLectureFilePathByIdAsync(Guid id);
    Task AddLectureAsync(Lecture lecture);
    Task DeleteLectureAsync(Lecture id);
}