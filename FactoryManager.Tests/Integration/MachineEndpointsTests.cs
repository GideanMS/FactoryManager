using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Tests.Infrastructure;

namespace FactoryManager.Tests.Integration;

public class MachineEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MachineEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostMachine_ShouldCreateMachine()
    {
        // Arrange
        var request = new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/machines", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}