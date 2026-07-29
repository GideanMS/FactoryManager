using FactoryManager.Application.Common.Sorting;

namespace FactoryManager.Application.Common.Pagination;

public abstract class QueryParameters
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }

    public int Offset => ((Page ?? 1) - 1) * (PageSize ?? 10);
}