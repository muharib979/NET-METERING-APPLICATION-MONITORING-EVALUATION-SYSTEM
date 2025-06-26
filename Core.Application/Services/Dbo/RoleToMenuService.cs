using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;
using Shared.DTOs.Enums;

namespace Core.Application.Services.Dbo;

public class RoleToMenuService : IRoleToMenuService
{
    private readonly IRoleToMenuRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMenuService _menuService;
    private readonly IRoleService _roleService;
    private readonly IMenuRepository _menuRepository;


    public RoleToMenuService(IRoleToMenuRepository repository,IRoleService roleService, IMapper mapper, IMenuService menuService, IMenuRepository menuRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _menuService = menuService;
        _roleService = roleService;
        _menuRepository = menuRepository;
    }

    public async Task<int> AssignRoleToMenu(List<RoleToMenu> rtm)
    {
        int status = 0;
        if (rtm == null)
            return status;

        status = await _repository.AssignRoleToMenu(rtm);

        return status;
    }

    public async Task<List<NavItemDto>> GetAllMenuHirerchyForRole(int roleId)
    {
        List<NavItemDto> dtres = new List<NavItemDto>();

        var menuList = await _repository.GetAllMenuHirerchyForRole(roleId.ToString());

        foreach (var item in menuList)
        {
            var navItem = _mapper.Map<NavItemDto>(item);

            navItem.Children = await GetChildMenuByParentRTM(item.ID.ToString(), roleId);
            if (navItem.Children.Count > 0)
            {
                int activeCount = navItem.Children.Count(x => x.IsActive == (int)BooleanEnum.FALSE);
                if (activeCount > 0)
                {
                    navItem.IsActive = (int)BooleanEnum.FALSE;
                }
                else
                {
                    navItem.IsActive = (int)BooleanEnum.TRUE;
                }
            }
            else
            {
                navItem.IsActive = item.IS_ACTIVE;
            }


            dtres.Add(navItem);
        }
        return dtres;
    }

    private async Task<List<NavItemDto>> GetChildMenuByParentRTM(string parentId, int roleId)
    {
        List<NavItemDto> childList = new List<NavItemDto>();

        var childMenuList = await _repository.GetChildMenuByParentRTM(parentId, roleId);

        foreach (var item in childMenuList)
        {
            var navItem = _mapper.Map<NavItemDto>(item);

            if (item.IS_PARENT == (int)BooleanEnum.TRUE)
            {
                navItem.Children = await GetChildMenuByParentRTM(item.ID.ToString(), roleId);

            }

            childList.Add(navItem);
        }
        return childList;
    }

    public async Task<List<SideBarItemDto>> GetAllRoleToMenuByRoleNUser(int roleId, int userId)
    {
        var role = await _roleService.GetByIdAsync(roleId);
        var groupMenus = _mapper.Map<List<SideBarItemDto>>(await _menuRepository.GetAllMenu());
        // await _menuService.GetAllGroupMenu();

        List<SideBarItemDto> menusWithChild = new List<SideBarItemDto>();

        var menus = await _repository.GetAllRoleToMenuByRoleNUser(roleId, userId);
        foreach (var item in menus)
        {
            var navItem = _mapper.Map<SideBarItemDto>(item);

            navItem.Children = await GetChildMenuByParentNRole(roleId, item.ID.ToString(), userId);

            if (navItem.Children.Count > 0)
            {
                foreach (var navChild in navItem.Children)
                {
                    navChild.Icon = null;

                    if (navChild.ItemId == role.MENU_ID_FK)
                    {
                        navChild.Home = (int)BooleanEnum.TRUE;
                    }
                }
            }
            if (navItem.Children.Count == 0) navItem.Children = null;

            menusWithChild.Add(navItem);

        }


        var sideBarItemMenu = new List<SideBarItemDto>();

        foreach (var group in groupMenus)
        {
            group.Icon = null;

            sideBarItemMenu.Add(group);
            //if(group.itemid == groupMenus[0].itemid)
            //{
            //    //sideBarItemMenu.Add(dashBoardMenus.FirstOrDefault());
            //}

            foreach (var menu in menusWithChild)
            {
                if (menu.ItemId == role.MENU_ID_FK)
                {
                    menu.Home = (int)BooleanEnum.TRUE;
                }
                if (menu.GroupId == group.ItemId.ToString())
                {
                    if (group.Title == "Alarm Actions")
                    {
                        menu.Badge = new BadgeSideBar { Text = "0" };
                    }
                    sideBarItemMenu.Add(menu);
                }
            }

        }
        //return sideBarItemMenu.OrderBy(x => x.ItemId).ToList();
        return sideBarItemMenu.OrderBy(x => x.GroupId).ToList();
    }

    private async Task<List<SideBarItemDto>> GetChildMenuByParentNRole(int roleId, string parentId, int userId)
    {
        List<SideBarItemDto> dtres = new List<SideBarItemDto>();

        var rtmChildList = await _repository.GetChildMenuByParentNRole(roleId, parentId, userId);
        foreach (var item in rtmChildList)
        {
            if (item.IsParent == (int)BooleanEnum.TRUE)
            {
                var navItem = _mapper.Map<SideBarItemDto>(item);
                navItem.Children = await GetChildMenuByParentNRole(roleId, item.MenuFkId.ToString(), userId);
                dtres.Add(navItem);
            }
            else
            {
                var navItem = _mapper.Map<SideBarItemDto>(item);
                dtres.Add(navItem);
            }
        }
        return dtres;
    }

    public async Task<List<RoleToMenu>> createScalerListRoletToMenu(RoleToMenuPostDto model)
    {
        List<RoleToMenu> rtmList = new List<RoleToMenu>();
        var menus = model.MenuItems;
        foreach (var item in menus)
        {
            RoleToMenu rtm = new RoleToMenu();
            rtm.ROLE_ID_FK = model.RoleId;
            rtm.MENU_ID_FK = item.ItemId;
            rtm.TIMESTAMP = DateTime.Now;
            if (item.Children != null)
            {
                if (item.Children.Count > 0)
                {
                    RoleToMenuPostDto child = new();
                    child.RoleId = model.RoleId;
                    child.MenuItems = item.Children;
                    rtmList.AddRange(await createScalerListRoletToMenu(child));
                    int activeCount = item.Children.Count(x => x.IsActive == (int)BooleanEnum.TRUE);
                    rtm.IS_ACTIVE = activeCount > 0 ? (int)BooleanEnum.TRUE : (int)BooleanEnum.FALSE;
                }
                else
                {
                    rtm.IS_ACTIVE = item.IsActive;
                }
            }
            else
            {
                rtm.IS_ACTIVE = item.IsActive;
            }

            rtmList.Add(rtm);

        }
        return rtmList;
    }
}
