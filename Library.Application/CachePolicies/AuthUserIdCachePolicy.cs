using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Library.Application.CachePolicies;

public class AuthUserIdCachePolicy : IOutputCachePolicy
{
    public static readonly AuthUserIdCachePolicy Instance = new();

    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        // Extract userId from the JWT token
        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        context.CacheVaryByRules.QueryKeys = $"userId:{userId}";

        // Add the tag to the cache rules
        var flag = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = flag;
        context.AllowCacheStorage = flag;
        context.AllowLocking = true;

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    ValueTask IOutputCachePolicy.ServeResponseAsync(OutputCacheContext context, CancellationToken cancellationToken)
    {
        var response = context.HttpContext.Response;
        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        if (response.StatusCode == 200)
            return ValueTask.CompletedTask;
        context.AllowCacheStorage = false;
        return ValueTask.CompletedTask;
    }

    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        var request = context.HttpContext.Request;
        return HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method);
    }
}