namespace Library.Domain.Models;

public class Book
{
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public Guid UploadedBy { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public Guid SubjectId { get; set; }
    public Subject Subject { get; set; }
}