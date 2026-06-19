using FactoryManager.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/machines", () =>
{
    var machines = new List<Machine>
    {
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Basic Furnace",
            ProductionPerMinute = 10,
            IsActive = true
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Assembler MK-1",
            ProductionPerMinute = 20,
            IsActive = true
        }
    };

    return machines;
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