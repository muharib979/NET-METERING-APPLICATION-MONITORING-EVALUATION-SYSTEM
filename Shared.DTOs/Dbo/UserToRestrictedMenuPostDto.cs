namespace Shared.DTOs.Dbo;

public class UserToRestrictedMenuPostDto
{
    public int UserId { get; set; }
    public List<NavItemDto> MenuItems { get; set; }
}
