using FactoryManager.Application.Common.Pagination;
using FactoryManager.Application.DTOs.Machines;

namespace FactoryManager.Application.Services.Interfaces;

public interface IMachineService
{
    Task<MachineResponse> CreateAsync(CreateMachineRequest request);
    Task<MachineResponse?> GetByIdAsync(Guid id);
    Task<MachineResponse?> UpdateAsync(Guid id, UpdateMachineRequest request);
    Task<MachineResponse> ActivateAsync(Guid id);
    Task<MachineResponse> DeactivateAsync(Guid id);
    Task<MachineResponse> StartMaintenanceAsync(Guid id);
    Task<MachineResponse> CompleteMaintenanceAsync(Guid id);
    Task DeleteAsync(Guid id);
    Task<PagedResult<MachineResponse>> GetAllAsync(MachineQueryParameters query);
}