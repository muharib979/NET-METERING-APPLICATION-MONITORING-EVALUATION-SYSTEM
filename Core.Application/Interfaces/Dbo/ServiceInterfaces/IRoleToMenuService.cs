namespace Core.Application.Interfaces.Dbo.ServiceInterfaces;

public interface IRoleToMenuService
{
    Task<List<SideBarItemDto>> GetAllRoleToMenuByRoleNUser(int roleId, int userId);
    Task<List<NavItemDto>> GetAllMenuHirerchyForRole(int roleId);
    Task<int> AssignRoleToMenu(List<RoleToMenu> rtm);
    Task<List<RoleToMenu>> createScalerListRoletToMenu(RoleToMenuPostDto model);
}
