namespace Library.Domain.Models;

public class NotificationModel
{
    public Guid NotificationId { get; set; } // Primary key

    public string Title { get; set; } = null!; // The message title

    public Guid RecipientUserId { get; set; } // Foreign key for the recipient user
    public User? RecipientUser { get; set; } // Navigation property for recipient

    public Guid SenderId { get; set; } // Foreign key for the sender user
    public User? SenderUser { get; set; } // Navigation property for sender

    public string Message { get; set; } = null!; // The message content
    public DateTime SentAt { get; set; } // When the notification was sent
    public bool IsRead { get; set; } // Whether the user has read the notification
}