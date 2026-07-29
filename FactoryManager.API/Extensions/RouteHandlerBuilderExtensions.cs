namespace FactoryManager.API.Filters;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder RequireApiKey(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ApiKeyEndpointFilter>();
    }
}