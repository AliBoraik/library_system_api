using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs.Subject;

public class SubjectDto : CreateSubjectDto
{
    [Required]
    public Guid SubjectId { get; set; }
}