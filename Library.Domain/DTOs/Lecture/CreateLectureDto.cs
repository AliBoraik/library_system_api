using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Domain.Attributes;
using Microsoft.AspNetCore.Http;

namespace Library.Domain.DTOs.Lecture;

public class CreateLectureDto
{
    [Required] public Guid SubjectId { get; set; }

    [Required] public string Title { get; set; } = null!;

    [Required] public string Description { get; set; } = null!;
    
    [Required(ErrorMessage = "Please select a file.")]
    [DataType(DataType.Upload)]
    [MaxFileSize(25 * 1024 * 1024)]
    [NotMapped]
    // [AllowedExtensions([".jpg", ".png"])]
    public IFormFile File { get; set; } = null!;
    
}