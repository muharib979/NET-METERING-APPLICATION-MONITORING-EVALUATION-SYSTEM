namespace Shared.DTOs.Dbo;

public class MenuDto
{
    public int? MenuId { get; set; }
    public string MenuName { get; set; }
    public string? Url { get; set; }
    public int? IsActive { get; set; }// bool
    public DateTime? TimeStamp { get; set; }
    public string? Icon { get; set; }
    public int? ParentId { get; set; }
    public int? IsParent { get; set; } // bool
    public int?  IsGroup { get; set; }// bool
    public int? GroupId { get; set; }
    public string? IconSVG { get; set; }
    public int TotalRowCount { get; set; }
    public int? OrderNo { get; set; }
}