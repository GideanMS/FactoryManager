using FactoryManager.Application.Common.Sorting;

namespace FactoryManager.Application.Common.Pagination;

public abstract class QueryParameters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    public int Offset => (Page - 1) * PageSize;
}