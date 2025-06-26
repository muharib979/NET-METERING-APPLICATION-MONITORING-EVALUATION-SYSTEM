namespace Core.Application.Interfaces.Dbo.ServiceInterfaces;

public interface IUserToMenuService
{
    Task<List<NavItemDto>> GetAllMenuHirerchyForUser(int userId);
    Task<int> AssignUserToMenu(List<UserToMenu> utm);
    Task<List<UserToMenu>> createScalerListUsertToMenu(UserToMenuPostDto model);
    Task<List<NavItemDto>> GetAllRestrictedMenuHirerchyForUser(int userId);
    Task<List<UserToRestrictedMenu>> createScalerListUserToRestrictedMenu(UserToRestrictedMenuPostDto model);
    Task<int> AssignUserToRestrictedMenu(List<UserToRestrictedMenu> utrmList);
}
