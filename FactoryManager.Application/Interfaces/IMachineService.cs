using FactoryManager.Application.DTOs.Machines;

namespace FactoryManager.Application.Services.Interfaces;

public interface IMachineService
{
    Task<MachineResponse> CreateAsync(CreateMachineRequest request);
    Task<MachineResponse?> GetByIdAsync(Guid id);
    Task<MachineResponse?> UpdateAsync(Guid id, UpdateMachineRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<List<MachineResponse>> GetAllAsync();
}