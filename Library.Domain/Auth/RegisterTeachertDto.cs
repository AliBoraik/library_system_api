using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Domain.Auth;

public class RegisterTeacherDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "DepartmentId is required")]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [NotMapped]
    public string ConfirmPassword { get; set; } = null!;
}