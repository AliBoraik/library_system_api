namespace Library.Domain.DTOs.Notification;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; init; } = null!;
    public string Message { get; init; } = null!;
    public long SentAt { get; init; }
    public bool IsRead { get; init; }
    public Guid SenderId { get; init; }
    public Guid RecipientUserId { get; init; }
}