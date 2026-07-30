using FactoryManager.API.Filters;

namespace FactoryManager.API.Extensions;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder RequireApiKey(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ApiKeyEndpointFilter>();
    }
}