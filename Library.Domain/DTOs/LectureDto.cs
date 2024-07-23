using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs;

public class LectureDto
{
    public int LectureId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string SubjectId { get; set; }
    [Required]
    public int UploadedBy { get; set; }
    public string FilePath { get; set; }
}