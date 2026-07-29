using FactoryManager.Application.Extensions;
using FactoryManager.Infrastructure.Extensions;
using FactoryManager.API.Extensions;
using FactoryManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddPresentation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FactoryDbContext>();
    dbContext.Database.Migrate();
}

app.UsePresentation();

app.Run();

public partial class Program
{
}