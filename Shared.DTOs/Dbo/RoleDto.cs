namespace Shared.DTOs.Dbo;

public class RoleDto
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public int IsSmsRole { get; set; } //bool
    public int MenuFkId { get; set; }
    public string MenuName { get; set; }
    public int IsActive { get; set; } //bool
    public int Priority { get; set; }
    public int TotalRowCount { get; set; }
}