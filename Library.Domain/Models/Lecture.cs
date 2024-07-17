namespace Library.Domain.Models;

public class Lecture
{
    public int LectureId { get; set; }
    public int SubjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public int UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Subject Subject { get; set; }
    public User User { get; set; }
}