using FactoryManager.Application.Extensions;
using FactoryManager.Infrastructure.Extensions;
using FactoryManager.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddPresentation();

var app = builder.Build();

app.UsePresentation();

app.Run();