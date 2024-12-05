namespace Library.Domain.DTOs.Notification;

public class NotificationRequest
{
    public Guid RecipientUserId { get; set; }
    public Guid SenderId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}
