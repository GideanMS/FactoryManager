using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Domain.Entities;

public static class MachineMapper
{
    public static MachineResponse ToResponse(Machine machine)
    {
        return new MachineResponse
        {
            Id = machine.Id,
            Name = machine.Name,
            ProductionPerMinute = machine.ProductionPerMinute,
            IsActive = machine.IsActive
        };
    }
}