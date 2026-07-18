using FactoryManager.Application.Common.Pagination;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Domain.Entities;

namespace FactoryManager.Application.Interfaces;

public interface IMachineRepository
{
    Task<PagedResult<Machine>> GetAllAsync(MachineQueryParameters query);
    Task<Machine?> GetByIdAsync(Guid id);
    Task AddAsync(Machine machine);
    Task SaveChangesAsync();
    void Remove(Machine machine);
}