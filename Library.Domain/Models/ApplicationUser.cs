using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ICollection<Book> Books { get; set; }
    public ICollection<Lecture> Lectures { get; set; }
}