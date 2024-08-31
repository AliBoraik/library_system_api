using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Book;

public class BookDto : CreateBookDto
{
    [Required] public Guid? BookId { get; set; }

    public DateTime UploadedAt { get; set; }
   
}