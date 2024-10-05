using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Models;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser<Guid>
{
    public virtual Student Student { get; init; } = null!;
    public virtual Teacher Teacher { get; init; } = null!;
}