namespace Library.Domain.Models;

public class UserNotification
{
    public Guid NotificationId { get; set; } // Foreign key for Notification
    public NotificationModel Notification { get; set; } = null!; // Navigation property

    public Guid UserId { get; set; } // Foreign key for User
    public User User { get; set; } = null!; // Navigation property

    public bool IsRead { get; set; } // Whether this user has read the notification
}