namespace Library.Domain.DTOs
{
    public class SubjectDto
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<LectureDto> Lectures { get; set; }
        public ICollection<BookDto> Books { get; set; }
    }
}