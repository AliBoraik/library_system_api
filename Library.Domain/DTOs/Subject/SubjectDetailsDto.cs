namespace Library.Domain.DTOs.Subject;

public class SubjectDetailsDto : SubjectDto
{
    public ICollection<LectureDto> Lectures { get; init; } = null!;
    public ICollection<BookDto> Books { get; init; } = null!;
}