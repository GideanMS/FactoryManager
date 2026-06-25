using FactoryManager.Application.DTOs.Machines;

namespace FactoryManager.Application.Services.Interfaces;

public interface IMachineService
{
    Task<MachineResponse> CreateAsync(CreateMachineRequest request);
    Task<List<MachineResponse>> GetAllAsync();
}