using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Models;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser<Guid>
{
    public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
    
    public int? DepartmentId { get; set; } 
    public Department Department { get; set; }
    
}