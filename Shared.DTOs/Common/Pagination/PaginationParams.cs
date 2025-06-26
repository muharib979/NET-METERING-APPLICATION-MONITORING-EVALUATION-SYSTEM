namespace Shared.DTOs.Common.Pagination;

public class PaginationParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } 
    private int _pageSize = 5;

    public int pageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string? SearchBy { get; set; }
    public string? SearchByCaseId { get; set; }
}
