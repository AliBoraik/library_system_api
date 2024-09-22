namespace Library.Domain.Models;

public class Lecture
{
    public Guid LectureId { get; set; }
    public Guid SubjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Subject Subject { get; set; }
    
    public Teacher Teacher { get; set; }
}