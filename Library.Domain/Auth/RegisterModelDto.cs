using System.ComponentModel.DataAnnotations;
using Library.Domain.Constants;

namespace Library.Domain.Auth;

public class RegisterModelDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; } = null!;
    

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$",
        ErrorMessage = StringConstants.IncorrectRequiredPasswordErrorMessage)]
    public string Password { get; set; } = null!;
}