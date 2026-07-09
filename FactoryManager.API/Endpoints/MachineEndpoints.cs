using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Application.Services.Interfaces;

namespace FactoryManager.API.Endpoints;

public static class MachineEndpoints
{
    public static void MapMachineEndpoints(this WebApplication app)
    {
        app.MapGet("/machines", async (IMachineService machineService) =>
        {
            var machines = await machineService.GetAllAsync();
            return Results.Ok(machines);
        })
        .WithName("GetAllMachines")
        .WithSummary("Gets all machines")
        .WithDescription("Retrieves a list of all production machines from the database")
        .Produces<IEnumerable<MachineResponse>>(StatusCodes.Status200OK);

        app.MapGet("/machines/{id:guid}", async (Guid id, IMachineService machineService) =>
        {
            var machine = await machineService.GetByIdAsync(id);

            if (machine is null)
                return Results.NotFound();

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
        .Produces(StatusCodes.Status500InternalServerError);
        

        app.MapPut("/machines/{id:guid}", async (Guid id, UpdateMachineRequest request, IMachineService machineService) =>
        {
            var machine = await machineService.UpdateAsync(id, request);

            if (machine is null)
                return Results.NotFound();

            return Results.Ok(machine);
        })
        .WithName("UpdateMachine")
        .WithSummary("Updates an existing machine")
        .WithDescription("Updates the details of an existing production machine in the database")
        .Produces<MachineResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/machines/{id:guid}", async (Guid id, IMachineService machineService) =>
        {
            var deleted = await machineService.DeleteAsync(id);

            if (!deleted)
                return Results.NotFound();

            return Results.NoContent();
        })
        .WithName("DeleteMachine")
        .WithSummary("Deletes a machine by its ID")
        .WithDescription("Deletes a machine from the database using its unique identifier")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}