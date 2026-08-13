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
    private readonly HttpClient _clientWithoutApiKey;

    public MachineEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _clientWithoutApiKey = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-Api-Key", "test-api-key");
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
    public async Task PostMachine_ShouldReturnBadRequest_WhenProductionExceedsMax()
    {
        // Arrange 
        var request = new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 120,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        };

        // Act
        var response = await _client.PostAsJsonAsync("/machines", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
    
    [Fact]
    public async Task GetMachines_ShouldReturnMachinesWithRunningStatus_WhenStatusIsRunning()
    {
        // Arrange
        var createResponse1 = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace 1",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created1 = await createResponse1.Content.ReadFromJsonAsync<MachineResponse>();

        var createResponse2 = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace 2",
            ProductionPerMinute = 60,
            MaxProductionPerMinute = 120,
            EnergyConsumptionPerMinute = 20,
            MaintenanceIntervalInDays = 25
        });
        var created2 = await createResponse2.Content.ReadFromJsonAsync<MachineResponse>();

        await _client.PatchAsync($"/machines/{created1.Id}/activate", null);

        // Act
        var getResponse = await _client.GetAsync("/machines?status=Running");
        var result = await getResponse.Content.ReadFromJsonAsync<PagedResult<MachineResponse>>();

        // Assert
        result!.Items.Should().ContainSingle(m => m.Id == created1.Id);
        result.Items.Should().NotContain(m => m.Id == created2.Id);
    }

    [Fact]
    public async Task PutMachine_ShouldReturnBadRequest_WhenProductionExceedsMax()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        var updateRequest = new UpdateMachineRequest
        {
            Name = "Updated Furnace",
            ProductionPerMinute = 150,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 20,
            MaintenanceIntervalInDays = 25
        };

        // Act
        var putResponse = await _client.PutAsJsonAsync($"/machines/{created.Id}", updateRequest);

        // Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PatchMachine_ShouldActivateMachine_WhenMachineIsOffline()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/activate", null);
        var activated = await patchResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        activated!.Status.Should().Be("Running");
    }

    [Fact]
    public async Task PutMachine_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();
        var updateRequest = new UpdateMachineRequest
        {
            Name = "Updated Furnace",
            ProductionPerMinute = 80,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 20,
            MaintenanceIntervalInDays = 25
        };

        // Act
        var putResponse = await _client.PutAsJsonAsync($"/machines/{machineId}", updateRequest);

        // Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PatchMachine_ShouldReturnBadRequest_WhenMachineIsUnderMaintenance()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/start-maintenance", null);
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/activate", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PatchMachineActivate_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();

        // Act
        var patchResponse = await _client.PatchAsync($"/machines/{machineId}/activate", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PatchMachine_ShouldDeactivateMachine_WhenMachineIsOnline()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/activate", null);
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/deactivate", null);
        var deactivated = await patchResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        deactivated!.Status.Should().Be("Offline");
    }
    
    [Fact]
    public async Task PatchMachineDeactivate_ShouldReturnBadRequest_WhenMachineIsUnderMaintenance()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/start-maintenance", null);
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/deactivate", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task PatchMachineDeactivate_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();

        // Act
        var patchResponse = await _client.PatchAsync($"/machines/{machineId}/deactivate", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PatchMachineStartMaintenance_ShouldStartMaintenance_WhenMachineIsOffline()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/start-maintenance", null);
        var maintenanceStarted = await patchResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        maintenanceStarted!.Status.Should().Be("Maintenance");
    }

    [Fact]
    public async Task PatchMachineStartMaintenance_ShouldReturnBadRequest_WhenMachineIsAlreadyUnderMaintenance()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/start-maintenance", null);
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/start-maintenance", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PatchMachineStartMaintenance_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();

        // Act
        var patchResponse = await _client.PatchAsync($"/machines/{machineId}/start-maintenance", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PatchMachineCompleteMaintenance_ShouldCompleteMaintenance_WhenMachineIsUnderMaintenance()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/start-maintenance", null);
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/complete-maintenance", null);
        var maintenanceCompleted = await patchResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        maintenanceCompleted!.Status.Should().Be("Offline");
    }

    [Fact]
    public async Task PatchMachineCompleteMaintenance_ShouldReturnBadRequest_WhenMachineIsNotUnderMaintenance()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/activate", null);
        var patchResponse = await _client.PatchAsync($"/machines/{created.Id}/complete-maintenance", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PatchMachineCompleteMaintenance_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();

        // Act
        var patchResponse = await _client.PatchAsync($"/machines/{machineId}/complete-maintenance", null);

        // Assert
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMachine_ShouldDelete_WhenMachineIsOffline()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/machines/{created.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var getResponse = await _client.GetAsync($"/machines/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMachine_ShouldReturnBadRequest_WhenMachineIsRunning()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        // Act
        await _client.PatchAsync($"/machines/{created.Id}/activate", null);
        var deleteResponse = await _client.DeleteAsync($"/machines/{created.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteMachine_ShouldReturnNotFound_WhenMachineDoesNotExist()
    {
        // Arrange
        var machineId = Guid.NewGuid();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/machines/{machineId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostApiKey_ShouldReturnUnauthorized_WhenApiKeyIsMissing()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/machines")
        {
            Content = JsonContent.Create(new CreateMachineRequest
            {
                Name = "Sem Chave",
                ProductionPerMinute = 50,
                MaxProductionPerMinute = 100,
                EnergyConsumptionPerMinute = 15,
                MaintenanceIntervalInDays = 30
            })
        };

        // Act
        var response = await _clientWithoutApiKey.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostApiKey_ShouldReturnUnauthorized_WhenApiKeyIsInvalid()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/machines")
        {
            Content = JsonContent.Create(new CreateMachineRequest
            {
                Name = "Chave Errada",
                ProductionPerMinute = 50,
                MaxProductionPerMinute = 100,
                EnergyConsumptionPerMinute = 15,
                MaintenanceIntervalInDays = 30
            })
        };
        request.Headers.Add("X-Api-Key", "chave-errada");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteWithoutApiKey_ShouldReturnUnauthorized_WhenApiKeyIsMissing()
    {
        // Arrange
        var createResponse = await _client.PostAsJsonAsync("/machines", new CreateMachineRequest
        {
            Name = "Steel Furnace",
            ProductionPerMinute = 50,
            MaxProductionPerMinute = 100,
            EnergyConsumptionPerMinute = 15,
            MaintenanceIntervalInDays = 30
        });
        var created = await createResponse.Content.ReadFromJsonAsync<MachineResponse>();

        var request = new HttpRequestMessage(HttpMethod.Delete, $"/machines/{created.Id}");

        // Act
        var response = await _clientWithoutApiKey.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}