namespace FactoryManager.API.Filters;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedApiKey = config["ApiKey"];

        if(string.IsNullOrWhiteSpace(expectedApiKey))
            return Results.Problem("API key is not configured on the server.", statusCode: StatusCodes.Status500InternalServerError);

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var provideKey) || provideKey != expectedApiKey)
            return Results.Unauthorized();

        return await next(context);
    }
}