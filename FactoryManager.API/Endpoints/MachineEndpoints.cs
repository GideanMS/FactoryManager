using FactoryManager.Application.Common.Pagination;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Application.Services.Interfaces;
using FactoryManager.API.Filters;
using FactoryManager.API.Extensions;

namespace FactoryManager.API.Endpoints;

public static class MachineEndpoints
{
    public static void MapMachineEndpoints(this WebApplication app)
    {
        app.MapGet("/machines", async ([AsParameters] MachineQueryParameters query, IMachineService service) =>
        {
            var machines = await service.GetAllAsync(query);
            return Results.Ok(machines);
        })
        .WithName("GetAllMachines")
        .WithSummary("Gets all machines")
        .WithDescription("Retrieves a list of all production machines from the database")
        .Produces<PagedResult<MachineResponse>>(StatusCodes.Status200OK);

        app.MapGet("/machines/{id:guid}", async (Guid id, IMachineService machineService) =>
        {
            var machine = await machineService.GetByIdAsync(id);


            return Results.Ok(machine);
        })
        .WithName("GetMachineById")
        .WithSummary("Gets a machine by its ID")
        .WithDescription("Retrieves a machine from the database using its unique identifier")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("/machines", async (
            CreateMachineRequest request,
            IMachineService machineService) =>
        {
            var machine = await machineService.CreateAsync(request);

            return Results.Created(
                $"/machines/{machine.Id}",
                machine);
        })
        .WithName("CreateMachine")
        .WithSummary("Creates a new machine")
        .WithDescription("Creates a new production machine and stores it in database")    
        .Produces<MachineResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();
        

        app.MapPut("/machines/{id:guid}", async (Guid id, UpdateMachineRequest request, IMachineService machineService) =>
        {
            var machine = await machineService.UpdateAsync(id, request);

            return Results.Ok(machine);
        })
        .WithName("UpdateMachine")
        .WithSummary("Updates an existing machine")
        .WithDescription("Updates the details of an existing production machine in the database")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();

        app.MapDelete("/machines/{id:guid}", async (Guid id, IMachineService machineService) =>
        {
            await machineService.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteMachine")
        .WithSummary("Deletes a machine by its ID")
        .WithDescription("Deletes a machine from the database using its unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();
        
        app.MapPatch("/machines/{id:guid}/activate", async (Guid id, IMachineService machineService) =>
        {
            var machine = await machineService.ActivateAsync(id);

            return Results.Ok(machine);
        })
        .WithName("ActivateMachine")
        .WithSummary("Activates a machine")
        .WithDescription("Activates a machine, changing its status to running")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();

        app.MapPatch("/machines/{id:guid}/deactivate", async (Guid id, IMachineService machineService) =>
        {
            var machine = await machineService.DeactivateAsync(id);

            return Results.Ok(machine);
        })
        .WithName("DeactivateMachine")
        .WithSummary("Deactivates a machine")
        .WithDescription("Deactivates a machine, changing its status to offline")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();

        app.MapPatch("/machines/{id:guid}/start-maintenance", async (Guid id, IMachineService machineService) =>
        {
            var machine = await machineService.StartMaintenanceAsync(id);

            return Results.Ok(machine);
        })
        .WithName("StartMaintenance")
        .WithSummary("Starts maintenance for a machine")
        .WithDescription("Initiates maintenance for a machine, changing its status to under maintenance")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();

        app.MapPatch("/machines/{id:guid}/complete-maintenance", async (Guid id, IMachineService machineService) =>
        {
            var machine = await machineService.CompleteMaintenanceAsync(id);

            return Results.Ok(machine);
        })
        .WithName("CompleteMaintenance")
        .WithSummary("Completes maintenance for a machine")
        .WithDescription("Completes maintenance for a machine, changing its status to active")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireApiKey();
    }
}