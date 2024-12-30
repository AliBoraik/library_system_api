using System.Security.Claims;
using Library.Domain.Constants;
using Library.Domain.DTOs.Notification;
using Library.Domain.Results;
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
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var notifications = await notificationService.GetNotificationsAsync(userGuid);
        return Ok(notifications);
    }

    /// <summary>
    ///     Retrieves all Unread notifications.
    /// </summary>
    [HttpGet("UnreadNotifications")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUnreadNotifications()
    {
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var notifications = await notificationService.GetUnreadNotificationsByUserIdAsync(userGuid);
        return Ok(notifications);
    }

    /// <summary>
    ///     Retrieves Limit notifications.
    /// </summary>
    [HttpGet("LimitNotifications")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetLimitNotifications([FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        if (page <= 0 || limit <= 0)
            return BadRequest(Errors.BadRequest("Page and limit must be greater than zero."));
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));

        var notifications = await notificationService.GetLimitNotificationsAsync(userGuid, page, limit);
        return Ok(notifications);
    }

    /// <summary>
    ///     Sends a notification to the specified recipients.
    /// </summary>
    [HttpPost("Send")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> SendNotification([FromBody] CreateNotificationDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var result = await notificationService.SendNotificationAsync(request);
        return result.Match<ActionResult>(
            Ok,
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    ///     Sends Bulk a notification to the specified recipients.
    /// </summary>
    [HttpPost("SendBulk")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult> SendBulkNotification([FromBody] CreateBulkNotificationDto request)
    {
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var result = await notificationService.SendBulkNotificationAsync(request);
        return result.Match<ActionResult>(
            Ok,
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    ///     Marks the specified notification as read.
    /// </summary>
    [HttpPatch("{notificationId}/Read")]
    public async Task<IActionResult> MarkNotificationRead([FromRoute] Guid notificationId)
    {
        // Extract userId from JWT token
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // Convert userId to Guid
        if (!Guid.TryParse(userIdClaim, out var userGuid)) return BadRequest(Errors.BadRequest("Invalid user ID."));
        var result = await notificationService.MarkNotificationReadAsync(notificationId, userGuid);
        return result.Match<ActionResult>(
            _ => Ok(),
            error => StatusCode(error.Code, error));
    }

    /// <summary>
    ///     Deletes a specific notification by its ID.
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