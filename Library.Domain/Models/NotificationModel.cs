namespace Library.Domain.Models;

public class NotificationModel
{
    public Guid Id { get; set; } // Primary key
    public string Title { get; set; } = null!; // The message title
    public string Message { get; set; } = null!; // The message content
    public DateTime SentAt { get; set; } // When the notification was sent
    public Guid SenderUserId { get; set; } // Foreign key for the sender user
    public User? SenderUser { get; set; } // Navigation property for sender
    public ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>(); // Many-to-many relationship

}