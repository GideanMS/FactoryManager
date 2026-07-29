namespace FactoryManager.API.Filters;

public class ApiKeyEndpointFilter : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedApiKey = config["ApiKey"];

        if(string.IsNullOrWhiteSpace(expectedApiKey))
            return await next(context);

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var provideKey) || provideKey != expectedApiKey)
            return Results.Unauthorized();

        return await next(context);
    }
}