namespace Library.Domain.DTOs.Book;

public class BookResponseDto
{
    public Guid? BookId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid SubjectId { get; set; }
    public DateTime UploadedAt { get; set; }
}