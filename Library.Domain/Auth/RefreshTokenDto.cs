using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Auth;

public class RefreshTokenDto
{
    [Required(ErrorMessage = "AccessToken is required")]
    public string AccessToken { get; set; } = null!;

    [Required(ErrorMessage = "RefreshToken is required")]
    public string RefreshToken { get; set; } = null!;
}