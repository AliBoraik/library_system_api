namespace Library.Domain.DTOs.Subject;

public class AddStudentToSubjectRequest
{
    public Guid StudentId { get; set; }
    public Guid SubjectId { get; set; }
}