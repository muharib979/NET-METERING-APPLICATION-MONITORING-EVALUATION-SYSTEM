using Core.Application.Commands.Dbo.AddRoleToMenu;

namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces;

public interface IRoleToMenuRepository
{
    Task<List<Menu>> GetAllRoleToMenuByRoleNUser(int roleId, int userId);
    Task<List<RoleToMenuDto>> GetChildMenuByParentNRole(int roleId, string parentId, int userId);
    Task<List<Menu>> GetAllMenuHirerchyForRole(string roleId);
    Task<List<Menu>> GetChildMenuByParentRTM(string parentId, int roleId);
    Task<int> AssignRoleToMenu(List<RoleToMenu> rtm);
}
