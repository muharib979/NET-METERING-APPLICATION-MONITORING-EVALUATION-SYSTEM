namespace Shared.DTOs.Common.Pagination;

public class PaginatedAlarmList<T>
{
    public PaginatedAlarmList(IEnumerable<T> items, int count, int pageNumber, int pageSize/*, int flagCount*/)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        //FlagCount = flagCount;
        Data = items.ToList();
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int FlagCount { get; set; }
    public List<T> Data { get; set; }

    public static PaginatedAlarmList<T> Create(List<T> source, int pageNumber, int pageSize, int count/*, int flagCount*/) => new PaginatedAlarmList<T>(source, count, pageNumber, pageSize/*,flagCount*/);
}

