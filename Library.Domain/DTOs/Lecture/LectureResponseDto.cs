namespace Library.Domain.DTOs.Lecture;

public class LectureResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid SubjectId { get; set; }
    public DateTime UploadedAt { get; set; }
}