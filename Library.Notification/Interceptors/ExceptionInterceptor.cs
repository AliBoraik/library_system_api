using Grpc.Core;
using Grpc.Core.Interceptors;
using Library.Domain.Constants;

namespace Library.Notification.Interceptors;

public class ExceptionInterceptor(ILogger<ExceptionInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            // Proceed with the gRPC call
            return await continuation(request, context);
        }
        catch (RpcException)
        {
            // Let known RpcExceptions pass through
            throw;
        }
        catch (Exception ex)
        {
            // Log unexpected errors
            logger.LogError(ex,
                "Unhandled exception in method {Method}. Client: {Client}. Request: {Request}. Exception: {Exception}",
                context.Method,
                context.Peer,
                request,
                ex.ToString());
            // Return a generic internal error to the client
            throw new RpcException(new Status(StatusCode.Internal, StringConstants.InternalServerError));
        }
    }
}