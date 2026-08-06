using FactoryManager.Domain.Entities;
using FactoryManager.Application.DTOs.Machines;
using FactoryManager.Application.Common.Sorting;

public static class MachineQueryableExtensions
{
    public static IQueryable<Machine> ApplyFilters(this IQueryable<Machine> query, MachineQueryParameters parameters)
    {
        if (!string.IsNullOrEmpty(parameters.Name))
        {
            query = query.Where(machine => machine.Name.Contains(parameters.Name));
        }

        if (parameters.Status.HasValue)
        {
            query = query.Where(machine => machine.Status == parameters.Status.Value);
        }

        if (parameters.MinProduction.HasValue)
        {
            query = query.Where(machine => machine.ProductionPerMinute >= parameters.MinProduction.Value);
        }

        if (parameters.MaxProduction.HasValue)
        {
            query = query.Where(machine => machine.ProductionPerMinute <= parameters.MaxProduction.Value);
        }

        return query;
    }

    public static IQueryable<Machine> ApplySorting(this IQueryable<Machine> query, MachineQueryParameters parameters)
    {
        if (string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            return query.OrderBy(machine => machine.Name);
        }

        if (!MachineSortExpressions.SortExpressions.TryGetValue(parameters.SortBy.ToLowerInvariant(), out var sortExpression))
        {
            return query.OrderBy(machine => machine.Name);
        }

        return parameters.SortDirection == SortDirection.Desc
            ? query.OrderByDescending(sortExpression)
            : query.OrderBy(sortExpression);
    }
}