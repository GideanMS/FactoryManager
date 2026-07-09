using FactoryManager.Application.Interfaces;
using FactoryManager.Infrastructure.Repositories;
using FactoryManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using FactoryManager.Application.Services.Interfaces;
using FactoryManager.Application.Services;
using FactoryManager.Application.DTOs.Machines;
using FluentValidation;
using FactoryManager.Application.Validators;
using FactoryManager.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IValidator<CreateMachineRequest>, CreateMachineValidator>();
builder.Services.AddScoped<IValidator<UpdateMachineRequest>, UpdateMachineValidator>();

builder.Services.AddDbContext<FactoryDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapMachineEndpoints();

app.MapGet("/", () =>
{
    return "Factory Manager API";
});

app.Run();