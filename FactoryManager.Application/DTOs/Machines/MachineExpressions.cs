using System.Linq.Expressions;
using FactoryManager.Domain.Entities;

namespace FactoryManager.Application.Common.Sorting;

public static class MachineSortExpressions
{
    public static readonly Dictionary<string, Expression<Func<Machine, object>>> SortExpressions = new()
    {
        ["name"] = machine => machine.Name,
        ["isActive"] = machine => machine.IsActive,
        ["production"] = machine => machine.ProductionPerMinute
    };
}