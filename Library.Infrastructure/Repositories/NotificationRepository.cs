using Library.Domain.Models;
using Library.Infrastructure.DataContext;
using Library.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class NotificationRepository(AppDbContext context) : INotificationRepository
{
    // Get all notifications for a specific user
    public async Task<IEnumerable<UserNotification>> FindNotificationsByUserIdAsync(Guid userId)
    {
        return await context.UserNotifications
            .Where(un => un.UserId == userId) // Filter by UserId
            .Include(un => un.Notification)
            .OrderByDescending(un => un.Notification.SentAt) // Order by SentAt
            .ToListAsync();
    }

    public async Task<IEnumerable<UserNotification>> FindLimitNotificationByUserIdAsync(Guid userId, int page,
        int limit)
    {
        return await  context.UserNotifications
            .Where(un => un.UserId == userId) // Filter by UserId
            .Include(un => un.Notification)
            .OrderByDescending(n => n.Notification.SentAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }
    
    
    // Get unread notifications for a user
    public async Task<IEnumerable<UserNotification>> FindUnreadNotificationsByUserIdAsync(Guid userId)
    {
        return await context.UserNotifications
            .Where(un => un.UserId == userId && !un.IsRead) // Filter by user and unread status
            .Include(un => un.Notification)
            .OrderByDescending(n => n.Notification.SentAt)

            .ToListAsync(); //
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
    
    public async Task<UserNotification?> FindNotificationByIdAndUserIdAsync(Guid notificationId, Guid userId)
    {
        return await context.UserNotifications
            .Where(un => un.UserId == userId && un.NotificationId == notificationId)
            .FirstOrDefaultAsync();
    }


    // Get a single notification by ID
    public async Task<NotificationModel?> FindNotificationByIdAsync(Guid notificationId)
    {
        return await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId);
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