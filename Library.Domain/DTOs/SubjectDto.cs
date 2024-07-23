namespace Library.Domain.DTOs
{
    public class SubjectDto
    {
        public Guid SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DepartmentId { get; set; }
        public ICollection<LectureDto> Lectures { get; set; }
        public ICollection<BookDto> Books { get; set; }
    }
}