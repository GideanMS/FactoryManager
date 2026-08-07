using FactoryManager.API.Endpoints;
using FactoryManager.API.Middlewares;

namespace FactoryManager.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseExceptionMiddleware();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapGet("/", () => Results.Redirect("/swagger"));

        app.UseHttpsRedirection();

        app.MapMachineEndpoints();

        return app;
    }
}