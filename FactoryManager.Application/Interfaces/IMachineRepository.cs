using FactoryManager.Domain.Entities;

namespace FactoryManager.Application.Interfaces;

public interface IMachineRepository
{
    Task<List<Machine>> GetAllAsync();
    Task<Machine?> GetByIdAsync(Guid id);
    Task AddAsync(Machine machine);
    Task SaveChangesAsync();
}