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
            .Where(n => n.UserNotifications.Any(un => un.UserId == userId)) // Filter notifications by userId
            .Include(n =>
                n.UserNotifications.Where(un => un.UserId == userId)) // Include only UserNotifications where Id matches
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<NotificationModel>> FindLimitNotificationByUserIdAsync(Guid userId, int page,
        int limit)
    {
        return await context.Notifications
            .Where(n => n.UserNotifications.Any(un => un.UserId == userId)) // Filter notifications by userId
            .Include(n =>
                n.UserNotifications.Where(un => un.UserId == userId)) // Include only UserNotifications where Id matches
            .OrderByDescending(n => n.SentAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }

    // Add a new notification
    public async Task<Guid> AddNotificationAsync(NotificationModel notification)
    {
        var result = await context.Notifications.AddAsync(notification);
        await SaveChangesAsync();
        return result.Entity.Id;
    }

    // Mark a notification as read
    public async Task MarkNotificationReadAsync(Guid notificationId, Guid userId)
    {
        // Find the specific UserNotification record for the given notification and user
        var userNotification = await context.UserNotifications
            .FirstOrDefaultAsync(un => un.NotificationId == notificationId && un.UserId == userId);

        if (userNotification != null)
        {
            // Mark as read
            userNotification.IsRead = true;
            await SaveChangesAsync();
        }
    }


    // Get a single notification by ID
    public async Task<NotificationModel?> FindNotificationByIdAsync(Guid notificationId)
    {
        return await context.Notifications
            .Include(n => n.UserNotifications)
            .FirstOrDefaultAsync(n => n.Id == notificationId);
    }

    // Get unread notifications for a user
    public async Task<IEnumerable<NotificationModel>> FindUnreadNotificationsByUserIdAsync(Guid userId)
    {
        return await context.Notifications
            .Where(n => n.UserNotifications.Any(un =>
                un.UserId == userId && !un.IsRead)) // Filter notifications by userId
            .Include(n =>
                n.UserNotifications.Where(un => un.UserId == userId)) // Include only UserNotifications where Id matches
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task DeleteNotificationAsync(NotificationModel notification)
    {
        context.Notifications.Remove(notification);
        await SaveChangesAsync();
    }

    // Save changes to the database
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}