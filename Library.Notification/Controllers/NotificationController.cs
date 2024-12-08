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
    ///     Retrieves all notifications.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized(StringConstants.UserIdMissing);
        var notifications = await notificationService.GetNotificationsAsync(userId);
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
        var result = await notificationService.MarkNotificationReadAsync(notificationId);
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }
}