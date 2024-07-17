using System.ComponentModel.DataAnnotations;

namespace Library.Domain.DTOs
{
    public class DepartmentDto
    {
        [Required]
        public int DepartmentId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        public ICollection<SubjectInfoDto> Subjects { get; set; }
    }
}