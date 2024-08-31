using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Book;

public class CreateBookDto
{
    [Required] public Guid SubjectId { get; set; }

    [Required] public string Title { get; set; } = null!;

    [Required] public string Description { get; set; } = null!;

    [Required] public Guid UploadedBy { get; set; }
}