using FactoryManager.Application.Interfaces;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Domain.Entities;
using FactoryManager.Application.Services.Interfaces;
using FactoryManager.Domain.Exceptions;
using FluentValidation;
using FactoryManager.Application.Common.Pagination;

namespace FactoryManager.Application.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _repository;
    private readonly IValidator<CreateMachineRequest> _createValidator;
    private readonly IValidator<UpdateMachineRequest> _updateValidator;

    public MachineService(IMachineRepository repository, IValidator<CreateMachineRequest> createValidator, IValidator<UpdateMachineRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<MachineResponse> CreateAsync(CreateMachineRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new DomainException(validationResult.Errors.First().ErrorMessage);

        var machine = new Machine(request.Name, request.ProductionPerMinute, request.MaxProductionPerMinute, request.EnergyConsumptionPerMinute, request.MaintenanceIntervalInDays);

        await _repository.AddAsync(machine);
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse?> GetByIdAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse?> UpdateAsync(Guid id, UpdateMachineRequest request)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        var validationResult = await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new DomainException(validationResult.Errors.First().ErrorMessage);

        machine.UpdateInformation(request.Name, request.ProductionPerMinute, request.MaxProductionPerMinute, request.EnergyConsumptionPerMinute, request.MaintenanceIntervalInDays);
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse> ActivateAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        machine.Activate();
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse> DeactivateAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        machine.Deactivate();
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse> CompleteMaintenanceAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        machine.CompleteMaintenance();
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
    }

    public async Task<MachineResponse> StartMaintenanceAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        machine.StartMaintenance();
        await _repository.SaveChangesAsync();

        return MachineMapper.ToResponse(machine);
    }

    public async Task<PagedResult<MachineResponse>> GetAllAsync(MachineQueryParameters query)
    {
        var PagedMachines = await _repository.GetAllAsync(query);

        return PagedMachines.Map(MachineMapper.ToResponse);
    }

    public async Task DeleteAsync(Guid id)
    {
        var machine = await _repository.GetByIdAsync(id);

        if (machine is null)
            throw new NotFoundException("Machine not found.");

        if (machine.Status == MachineStatus.Running)
            throw new DomainException("Cannot delete a running machine.");
        
        _repository.Remove(machine);
        await _repository.SaveChangesAsync();
    }
}