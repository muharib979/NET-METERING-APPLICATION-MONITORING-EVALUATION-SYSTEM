namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces;

public interface IUserToMenuRepository
{
    Task<List<Menu>> GetAllMenuHirerchyForUser(int userId);
    Task<List<Menu>> GetChildMenuByParentUTM(string parentId, int userId);
    Task<int> AssignUserToMenu(List<UserToMenu> utm);
    Task<List<Menu>> GetAllRestrictedMenuHirerchyForUser(int userId);
    Task<List<Menu>> GetChildRestrictedMenuByParentUTM(string parentId, int userId);
    Task<int> AssignUserToRestrictedMenu(List<UserToRestrictedMenu> utrmList);
}
