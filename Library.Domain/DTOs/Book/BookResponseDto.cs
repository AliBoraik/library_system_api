namespace Library.Domain.DTOs.Book;

public class BookResponseDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int SubjectId { get; set; }
    public DateTime UploadedAt { get; set; }
}