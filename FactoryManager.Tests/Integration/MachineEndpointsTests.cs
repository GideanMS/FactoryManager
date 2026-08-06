using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Tests.Infrastructure;
using FactoryManager.Application.Common.Pagination;
using Microsoft.Extensions.DependencyInjection;
using FactoryManager.Infrastructure.Persistence;

namespace FactoryManager.Tests.Integration;

public class MachineEndpointsTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MachineEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FactoryDbContext>();
        db.Machines.RemoveRange(db.Machines);
        await db.SaveChangesAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task PostMachine_ShouldCreateMachine()
    {
        // Arrange
        var request = new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        };

        // Act
        var postResponse = await _client.PostAsJsonAsync("/machines", request);
        var created = await postResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Assert
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        created.Should().NotBeNull();
        created!.Name.Should().Be("Steel Furnace");
        created.ProductionPerMinute.Should().Be(50);
    }

    [Fact]
    public async Task PostMachine_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange 
        var request = new CreateMachineRequest
        {
            Name = "",
            ProductionPerMinute = 80,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        };

        // Act
        var response = await _client.PostAsJsonAsync("/machines", request);

        // Assert
        response.StatusCode
        .Should()
        .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetMachines_ShouldReturnCreatedMachine()
    {
        await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 80,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });

        var getResponse = await _client.GetAsync("/machines?Page=1&PageSize=10");
        var result = await getResponse.Content.ReadFromJsonAsync<PagedResult<MachineResponse>>();

        result!.Items.Should().ContainSingle(m => m.Name == "Steel Furnace");
    }

    [Fact]
    public async Task GetMachineById_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/machines/{machineId}");

        // Assert
        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }
}