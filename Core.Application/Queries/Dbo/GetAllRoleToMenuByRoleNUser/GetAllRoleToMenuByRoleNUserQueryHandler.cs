using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Enums;
using System.Collections.Generic;

namespace Core.Application.Queries.Dbo.GetAllRoleToMenuByRoleNUser;

public class GetAllRoleToMenuByRoleNUserQueryHandler : IRequestHandler<GetAllRoleToMenuByRoleNUserQuery, Response<List<SideBarItemDto>>>
{
    //private readonly IRoleToMenuService _service;
    private readonly IRoleToMenuRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMenuService _menuService;
    private readonly IRoleService _roleService;

    //public GetAllRoleToMenuByRoleNUserQueryHandler(IRoleToMenuService service) => _service = service;
    public GetAllRoleToMenuByRoleNUserQueryHandler(IRoleToMenuRepository repository, IMapper mapper, IMenuService menuService, IRoleService roleService) 
    {
        _repository= repository;
        _mapper= mapper;
        _menuService= menuService;
        _roleService= roleService;
    }

    public async Task<Response<List<SideBarItemDto>>> Handle(GetAllRoleToMenuByRoleNUserQuery request, CancellationToken cancellationToken) {
        var result = await GetAllRoleToMenuByRoleNUser(request.RoleId, request.UserId);
        return Response<List<SideBarItemDto>>.Success(result, "Successfully Retrived All Role To Menu By Role and User");
    }

    public async Task<List<SideBarItemDto>> GetAllRoleToMenuByRoleNUser(int roleId, int userId)
    {
        var role = await _roleService.GetByIdAsync(roleId);
        var groupMenus = await _menuService.GetAllGroupMenu();

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
                    //navChild.Icon = null;

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
}
