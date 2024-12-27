using System.Security.Claims;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Library.Application.CachePolicies
{
    public class AuthUserIdCachePolicy : IOutputCachePolicy
    {
        public static readonly AuthUserIdCachePolicy Instance = new();
        
        public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            // Extract userId from the JWT token
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            context.CacheVaryByRules.QueryKeys = $"userId:{userId}";

            context.EnableOutputCaching = true;
            context.AllowCacheLookup = true;
            context.AllowCacheStorage = true;
            context.AllowLocking = true;

            return ValueTask.CompletedTask;
        }

        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
        {
            var response = context.HttpContext.Response;

            // Prevent caching responses with Set-Cookie headers or non-200 status codes
            if (response.StatusCode != 200 || !StringValues.IsNullOrEmpty(response.Headers.SetCookie))
            {
                context.AllowCacheStorage = false;
            }

            return ValueTask.CompletedTask;
        }
    }
}