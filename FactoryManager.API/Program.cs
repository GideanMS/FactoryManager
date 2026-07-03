using FactoryManager.Domain.Entities;
using FactoryManager.Application.Interfaces;
using FactoryManager.Infrastructure.Repositories;
using FactoryManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FactoryManager.Application.Services.Interfaces;
using FactoryManager.Application.Services;
using FactoryManager.Application.DTOs.Machines;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<FactoryDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineService, MachineService>();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/machines", async (IMachineService machineService) =>
{
    return await machineService.GetAllAsync();
});

app.MapGet("/machines/{id:guid}", async (Guid id, IMachineService machineService) =>
{
    var machine = await machineService.GetByIdAsync(id);

    if (machine is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(machine);
});

app.MapPut("/machines/{id:guid}", async (Guid id, UpdateMachineRequest request, IMachineService machineService) =>
{
    var machine = await machineService.UpdateAsync(id, request);

    if (machine is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(machine);
});

app.MapPost("/machines", async (CreateMachineRequest request,IMachineService machineService) =>
{
    var machine =
        await machineService.CreateAsync(request);

    return Results.Created(
        $"/machines/{machine.Id}",
        machine);
});

app.MapGet("/", () =>
{
    return "Factory Manager API";
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}