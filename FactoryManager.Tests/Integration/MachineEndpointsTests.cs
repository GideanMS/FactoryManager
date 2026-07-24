using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Tests.Infrastructure;
using FactoryManager.Application.Common.Pagination;

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
        var postResponse = await _client.PostAsJsonAsync("/machines", request);

        var getResponse = await _client.GetAsync("/machines?Page=1&PageSize=10&SortDirection=Asc");

        var result = await getResponse.Content.ReadFromJsonAsync<PagedResult<MachineResponse>>();

        // Assert
        postResponse.StatusCode
        .Should()
        .Be(HttpStatusCode.Created);

        getResponse.StatusCode
        .Should()
        .Be(HttpStatusCode.OK);

        result.Should().NotBeNull();

        result!.Items.Should()
        .ContainSingle();

        var machine = result.Items.First();

        machine.Name.Should()
        .Be("Steel Furnace");

        machine.ProductionPerMinute.Should()
        .Be(50);
    }

    [Fact]
    public async Task PostMachine_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange 
        var request = new CreateMachineRequest
        {
            Name = "",
            ProductionPerMinute = -10
        };

        // Act
        var response = await _client.PostAsJsonAsync("/machines", request);

        // Assert
        response.StatusCode
        .Should()
        .Be(HttpStatusCode.BadRequest);
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