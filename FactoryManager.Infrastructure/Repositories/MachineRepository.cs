using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Application.Interfaces;
using FactoryManager.Domain.Entities;
using FactoryManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FactoryManager.Infrastructure.Repositories;

public class MachineRepository : IMachineRepository
{
    private readonly FactoryDbContext _context;

    public MachineRepository(FactoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Machine>> GetAllAsync(MachineQueryParameters query)
    {
        return await _context.Machines
        .AsNoTracking()
        .OrderBy(machine => machine.Name)
        .Skip(query.Offset)
        .Take(query.PageSize)
        .ToListAsync();
    }

    public async Task<Machine?> GetByIdAsync(Guid id)
    {
        return await _context.Machines.FindAsync(id);
    }

    public async Task AddAsync(Machine machine)
    {
        await _context.Machines.AddAsync(machine);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Remove(Machine machine)
    {
        _context.Machines.Remove(machine);
    }
}