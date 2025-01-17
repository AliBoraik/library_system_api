using System.Security.Claims;

namespace Library.Domain.Extensions;

public static class UserExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID claim is missing.");
        if (Guid.TryParse(userIdClaim, out var userGuid))
            return userGuid;

        throw new UnauthorizedAccessException("Invalid user ID.");
    }
}