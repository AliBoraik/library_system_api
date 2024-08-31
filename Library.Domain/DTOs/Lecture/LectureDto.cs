using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.Domain.DTOs.Lecture;

public class LectureDto : CreateLectureDto
{
    [Required] public Guid? LectureId { get; set; }

    public DateTime UploadedAt { get; set; }
}