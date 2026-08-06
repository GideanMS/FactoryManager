using FactoryManager.Application.Common.Pagination;

namespace FactoryManager.Application.DTOs.Machines;

public class MachineQueryParameters : QueryParameters
{
    public string? Name { get; set; }
    public MachineStatus? Status { get; set; }
    public decimal? MinProduction { get; set; }
    public decimal? MaxProduction { get; set; }
}