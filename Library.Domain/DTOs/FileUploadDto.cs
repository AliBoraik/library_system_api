using System.ComponentModel.DataAnnotations;
using Library.Domain.Attributes;
using Microsoft.AspNetCore.Http;

namespace Library.Domain.DTOs;

public class FileUploadDto
{
    [Required(ErrorMessage = "Please select a file.")]
    [DataType(DataType.Upload)]
    [MaxFileSize(100 * 1024 * 1024)]
    // [AllowedExtensions([".jpg", ".png"])]
    public IFormFile File { get; set; } = null!;
}