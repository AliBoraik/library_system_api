using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    // Get all notifications for a specific user
    public async Task<IEnumerable<NotificationModel>> FindNotificationsByUserIdAsync(Guid userId)
    {
        return await context.Notifications
            .Where(n => n.RecipientUserId == userId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    // Add a new notification
    public async Task<Guid> AddNotificationAsync(NotificationModel notification)
    {
        var result = await context.Notifications.AddAsync(notification);
        return result.Entity.RecipientUserId;
    }

    // Mark a notification as read
    public async Task MarkNotificationReadAsync(Guid notificationId)
    {
        var notification = await context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            context.Notifications.Update(notification);
        }
    }

    // Get a single notification by ID
    public async Task<NotificationModel?> FindNotificationByIdAsync(Guid notificationId)
    {
        return await context.Notifications.FindAsync(notificationId);
    }

    // Get unread notifications for a user
    public async Task<IEnumerable<NotificationModel>> FindUnreadNotificationsByUserIdAsync(Guid userId)
    {
        return await context.Notifications
            .Where(n => n.RecipientUserId == userId && !n.IsRead)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    // Save changes to the database
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}