using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Models;

public class NotificationModel
{
    [BsonId]
    public ObjectId Id { get; set; } // MongoDB unique identifier
    public required string Title { get; init; } // The message content
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid RecipientUserId { get; set; } // Who will receive the notification
    public required string Message { get; init; } // The message content
    public DateTime SentAt { get; init; } // When the notification was sent
    public bool IsRead { get; init; } // Whether the user has read the notification
    
}