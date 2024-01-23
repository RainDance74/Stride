using Microsoft.Extensions.Logging;

namespace Stride.Application.Common.Behaviors;

public class UnhandledExceptionBehaviour<TRequest, TResponse>
    (ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch(Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "Stride Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
