namespace Shared.DTOs.Dbo;

public class UserToMenuPostDto
{
    public int UserId { get; set; }
    public List<NavItemDto> MenuItems { get; set; }
}
