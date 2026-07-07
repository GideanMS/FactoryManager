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

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse?> GetByIdAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
        {
            return null;
        }

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse?> UpdateAsync(Guid id, UpdateMachineRequest request)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
        {
            throw new Exception("Machine not found.");
        }

        machine.UpdateInformation(request.Name, request.ProductionPerMinute);
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
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

    public async Task<bool> DeleteAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            return false;
        
        _repository.Remove(machine);
        await _repository.SaveChangesAsync();

        return true;
    }
}