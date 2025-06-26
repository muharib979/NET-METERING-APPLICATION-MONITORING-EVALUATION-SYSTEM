using Oracle.ManagedDataAccess.Client;

namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces;

public interface IMenuRepository : IBaseRepository<Menu>
{
    Task<List<Menu>> GetAllDashBoardMenuDDAsync();
    Task<List<DropdownResult>> GetAllParentMenuDDAsync();
    Task<List<DropdownResult>> GetGroupDDAsync();
    Task<List<Menu>> GetAllMenu();
    Task<List<Menu>> GetAllDashBoardMenu(int roleId);
    Task<List<SVGICon>> GetSVGIconList();
    Task<int> DeleteMenuById(int id);
 
}
