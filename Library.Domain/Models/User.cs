using Microsoft.AspNetCore.Identity;

namespace Library.Domain.Models;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser<Guid>
{
    public virtual ICollection<NotificationModel> SentNotifications { get; set; } =
        null!; // Notifications sent by the user

    public ICollection<NotificationModel> ReceivedNotifications { get; set; } =
        null!; // Notifications received by the user
}