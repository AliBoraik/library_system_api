using System.Security.Claims;
using Library.Domain.Constants;
using Library.Domain.DTOs.Notification;
using Library.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Notification.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    /// <summary>
    /// Retrieves all notifications.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine(userId);
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var notifications = await notificationService.GetNotificationsAsync(Guid.Parse(userId));
        return Ok(notifications);
    }
    
    /// <summary>
    /// Retrieves all Unread notifications.
    /// </summary>
    [HttpGet("UnreadNotifications")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUnreadNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var notifications = await notificationService.GetUnreadNotificationsByUserIdAsync(Guid.Parse(userId));
        return Ok(notifications);
    }

    /// <summary>
    /// Sends a notification to the specified recipients.
    /// </summary>
    [HttpPost("Send")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> SendNotification([FromBody] CreateNotificationDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var result = await notificationService.SendNotificationAsync(request);
        return result.Match<ActionResult>(
            Ok,
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    /// Marks the specified notification as read.
    /// </summary>
    [HttpPatch("{notificationId}/Read")]
    public async Task<IActionResult> MarkNotificationRead([FromRoute] Guid notificationId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var result = await notificationService.MarkNotificationReadAsync(notificationId ,  Guid.Parse(userId));
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }
    /// <summary>
    /// Deletes a specific notification by its ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        var result = await notificationService.DeleteNotificationByIdAsync(id);
        if (!result.IsOk) return StatusCode(result.Error.Code, result.Error);
        return Ok();
    }
}