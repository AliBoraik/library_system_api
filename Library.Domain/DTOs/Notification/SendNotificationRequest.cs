namespace Library.Domain.DTOs.Notification;

public class SendNotificationRequest
{
    public Guid RecipientUserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}
