namespace Shared.DTOs.Common.Pagination;

public class PaginatedList<T>
{
    public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        Data = items.ToList();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public List<T> Data { get; set; }

    public static PaginatedList<T> Create(List<T> source, int pageNumber, int pageSize, int count) => new PaginatedList<T>(source, count, pageNumber, pageSize);
}

