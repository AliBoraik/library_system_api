namespace Library.Domain.DTOs.Notification;

public class CreateBulkNotificationDto
{
    public required string Title { get; init; }
    public required string Message { get; init; }
    public required Guid SenderId { get; init; }
    public required IEnumerable<Guid> RecipientUserIds { get; init; } // Supports multiple recipient IDs
}