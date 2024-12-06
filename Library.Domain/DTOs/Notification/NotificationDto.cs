namespace Library.Domain.DTOs.Notification;

public class NotificationDto
{
    public Guid NotificationId { get; set; }
    public string Title { get; init; } = null!;
    public string Message { get; init; } = null!;
    public DateTime SentAt { get; init; }
    public bool IsRead { get; init; }
    public Guid SenderId { get; init; }
    public Guid RecipientUserId { get; init; }
}