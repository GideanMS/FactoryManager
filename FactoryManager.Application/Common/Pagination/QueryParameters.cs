namespace FactoryManager.Application.Common.Pagination;

public abstract class QueryParameters
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;

    public int Offset => (Page - 1) * PageSize;
}