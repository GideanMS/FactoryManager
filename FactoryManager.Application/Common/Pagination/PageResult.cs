namespace FactoryManager.Application.Common.Pagination;

public sealed class PageResult<T>
{
    public IReadOnlyList<T> Items {get;}
    public int CurrentPage {get;}
    public int PageSize {get;}
    public int TotalCount {get;}
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    public PageResult <TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper);

        return new PageResult<TResult>(
            Items.Select(mapper).ToList(),
            CurrentPage,
            PageSize,
            TotalCount);
    }

    public PageResult(IReadOnlyList<T> items, int currentPage, int pageSize, int totalCount)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (currentPage <= 1)
            throw new ArgumentOutOfRangeException(nameof(currentPage));
        if (pageSize <= 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        if (totalCount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalCount));

        Items = items;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}