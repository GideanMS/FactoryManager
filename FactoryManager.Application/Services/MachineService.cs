using FactoryManager.Application.Interfaces;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Domain.Entities;
using FactoryManager.Application.Services.Interfaces;

namespace FactoryManager.Application.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _repository;

    public MachineService(IMachineRepository repository)
    {
        _repository = repository;
    }

    public async Task<MachineResponse> CreateAsync(CreateMachineRequest request)
    {
        var machine = new Machine(request.Name, request.ProductionPerMinute);

        await _repository.AddAsync(machine);
        await _repository.SaveChangesAsync();

        return new MachineResponse
        {
            Id = machine.Id,
            Name = machine.Name,
            ProductionPerMinute = machine.ProductionPerMinute,
            IsActive = machine.IsActive
        };
    }

    public async Task<List<MachineResponse>> GetAllAsync()
    {
        var machines = await _repository.GetAllAsync();
        return machines.Select(m => new MachineResponse
        {
            Id = m.Id,
            Name = m.Name,
            ProductionPerMinute = m.ProductionPerMinute,
            IsActive = m.IsActive
        }).ToList();
    }
}