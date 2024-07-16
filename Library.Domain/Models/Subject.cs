namespace Library.Domain.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Department Department { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}