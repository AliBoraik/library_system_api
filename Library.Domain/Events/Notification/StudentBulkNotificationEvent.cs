namespace Library.Domain.Events.Notification;

public class StudentBulkNotificationEvent
{
    public required string Title { get; init; }
    public required string Message { get; init; }
    public required Guid SenderId { get; init; }
    public required int DepartmentId { get; init; } 
}