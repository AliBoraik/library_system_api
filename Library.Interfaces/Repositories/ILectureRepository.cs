using Library.Domain.Models;

namespace Library.Interfaces.Repositories
{
    public interface ILectureRepository
    {
        Task<IEnumerable<Lecture>> GetAllLecturesAsync();
        Task<Lecture?> GetLectureByIdAsync(int id);
        Task AddLectureAsync(Lecture? lecture);
        Task UpdateLectureAsync(Lecture lecture);
        Task DeleteLectureAsync(int id);
    }
}