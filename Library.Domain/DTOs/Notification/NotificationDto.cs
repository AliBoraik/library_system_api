namespace Library.Domain.DTOs.Notification;

public class NotificationDto
{
    public string Id { get; set; }
    public string Message { get; set; }
    public string Title { get; set; }
    public Guid SenderId { get; set; } 
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}