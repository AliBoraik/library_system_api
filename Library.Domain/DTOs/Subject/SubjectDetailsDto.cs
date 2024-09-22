using System.Text.Json.Serialization;
using Library.Domain.DTOs.Book;
using Library.Domain.DTOs.Lecture;

namespace Library.Domain.DTOs.Subject;

public class SubjectDetailsDto : SubjectDto
{
    [JsonPropertyOrder(2)] public ICollection<LectureResponseDto> Lectures { get; init; } = null!;

    [JsonPropertyOrder(3)] public ICollection<BookResponseDto> Books { get; init; } = null!;
}