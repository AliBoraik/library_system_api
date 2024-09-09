using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Auth;

public class LoginModelDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}