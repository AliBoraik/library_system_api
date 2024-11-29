using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Models;

public class NotificationModel
{
    [BsonId]
    public ObjectId Id { get; set; } // MongoDB unique identifier
    public string RecipientUserId { get; set; } // Who will receive the notification
    public string Message { get; set; } // The message content
    public DateTime SentAt { get; set; } // When the notification was sent
    public bool IsRead { get; set; } // Whether the user has read the notification
    
}