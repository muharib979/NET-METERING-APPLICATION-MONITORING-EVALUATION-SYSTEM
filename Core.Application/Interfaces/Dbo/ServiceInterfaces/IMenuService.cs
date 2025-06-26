namespace Core.Application.Interfaces.Dbo.ServiceInterfaces;

public interface IMenuService : IBaseService<MenuDto>
{
    Task<List<DropdownResult>> GetAllDashBoardMenuDDAsync();
    Task<List<DropdownResult>> GetAllParentMenuDDAsync();
    Task<List<DropdownResult>> GetGroupDDAsync();
    Task<List<SideBarItemDto>> GetAllGroupMenu();
    Task<List<SideBarItemDto>> GetAllDashBoardMenu(int roleId);
    Task<List<SVGICon>> GetSVGIconList();
}
