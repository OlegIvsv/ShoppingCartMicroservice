using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Api.Middleware;

public class ErrorHandlingMiddleware : IMiddleware 
{
    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            if(_logger.IsEnabled(LogLevel.Critical))
                _logger.LogCritical(ex, "An unexpected error occured");
            await WriteResponseWithCode500(context, ex);
        }
    }

    private async Task WriteResponseWithCode500(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occured",
            Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
            Detail = ex.Message
        };
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}