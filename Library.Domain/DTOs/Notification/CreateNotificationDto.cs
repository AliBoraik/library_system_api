namespace Library.Domain.DTOs.Notification;

public class CreateNotificationDto
{
    public required string Title { get; init; }
    public required string Message { get; init; }
    public required Guid SenderId { get; init; }
    public required Guid RecipientUserId { get; init; }
}