using System.Text.Json;
using FactoryManager.Domain.Exceptions;

namespace FactoryManager.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain exception occurred.");
            await HandleExceptionAsync(context, ex.Message, StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, "An unexpected error occurred.", StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new { error = message };
        var jsonResponse = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(jsonResponse);
    }
}