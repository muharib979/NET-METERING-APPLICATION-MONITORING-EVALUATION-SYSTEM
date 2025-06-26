namespace Shared.DTOs.Dbo;

public class RoleToMenuPostDto
{
    public int RoleId { get; set; }
    public List<NavItemDto> MenuItems { get; set; }
}
