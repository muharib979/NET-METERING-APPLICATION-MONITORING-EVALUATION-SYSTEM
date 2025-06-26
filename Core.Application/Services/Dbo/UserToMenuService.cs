using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;
using Shared.DTOs.Enums;

namespace Core.Application.Services.Dbo;

public class UserToMenuService : IUserToMenuService
{
    private readonly IUserToMenuRepository _repository;
    private readonly IMapper _mapper;

    public UserToMenuService(IUserToMenuRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<int> AssignUserToMenu(List<UserToMenu> utm)
    {
        int status = 0;
        if (utm == null)
            return status;

        status = await _repository.AssignUserToMenu(utm);

        return status;
    }

    public async Task<int> AssignUserToRestrictedMenu(List<UserToRestrictedMenu> utrmList)
    {
        int status = 0;
        if (utrmList == null)
            return status;

        status = await _repository.AssignUserToRestrictedMenu(utrmList);

        return status;
    }

    public async Task<List<UserToMenu>> createScalerListUsertToMenu(UserToMenuPostDto model)
    {
        List<UserToMenu> utmList = new List<UserToMenu>();
        var menus = model.MenuItems;
        foreach (var item in menus)
        {
            UserToMenu utm = new UserToMenu();
            utm.USER_ID_FK = model.UserId;
            utm.MENU_ID_FK = item.ItemId;
            utm.TIMESTAMP = DateTime.Now;
            if (item.Children != null)
            {
                if (item.Children.Count > 0)
                {
                    UserToMenuPostDto child = new();
                    child.UserId = model.UserId;
                    child.MenuItems = item.Children;
                    utmList.AddRange(await createScalerListUsertToMenu(child));
                    int activeCount = item.Children.Count(x => x.IsActive == (int)BooleanEnum.TRUE);
                    utm.IS_ACTIVE = activeCount > 0 ? (int)BooleanEnum.TRUE : (int)BooleanEnum.FALSE;
                }
                else
                {
                    utm.IS_ACTIVE = item.IsActive;
                }
            }
            else
            {
                utm.IS_ACTIVE = item.IsActive;
            }

            utmList.Add(utm);

        }
        return utmList;
    }

    public async Task<List<NavItemDto>> GetAllMenuHirerchyForUser(int userId)
    {
        var dtres = new List<NavItemDto>();

        var menuList = await _repository.GetAllMenuHirerchyForUser(userId);

        foreach (var item in menuList)
        {
            var navItem = _mapper.Map<NavItemDto>(item);

            navItem.Children = await GetChildMenuByParentUTM(item.ID.ToString(), userId); ;
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

    public async Task<List<NavItemDto>> GetAllRestrictedMenuHirerchyForUser(int userId)
    {
        var dtres = new List<NavItemDto>();

        var menuList = await _repository.GetAllRestrictedMenuHirerchyForUser(userId);

        foreach (var item in menuList)
        {
            var navItem = _mapper.Map<NavItemDto>(item);

            navItem.Children = await GetChildRestrictedMenuByParentUTM(item.ID.ToString(), userId); ;
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

    private async Task<List<NavItemDto>> GetChildRestrictedMenuByParentUTM(string parentId, int userId)
    {
        var childList = new List<NavItemDto>();

        var childMenuList = await _repository.GetChildRestrictedMenuByParentUTM(parentId, userId);

        foreach (var item in childMenuList)
        {
            var navItem = _mapper.Map<NavItemDto>(item);

            if (item.IS_PARENT == (int)BooleanEnum.TRUE)
            {
                navItem.Children = await GetChildRestrictedMenuByParentUTM(item.ID.ToString(), userId);

            }

            childList.Add(navItem);
        }
        return childList;
    }

    private async Task<List<NavItemDto>> GetChildMenuByParentUTM(string parentId, int userId)
    {
        var childList = new List<NavItemDto>();

        var childMenuList = await _repository.GetChildMenuByParentUTM(parentId, userId);

        foreach (var item in childMenuList)
        {
            var navItem = _mapper.Map<NavItemDto>(item);

            if (item.IS_PARENT == (int)BooleanEnum.TRUE)
            {
                navItem.Children = await GetChildMenuByParentUTM(item.ID.ToString(), userId);

            }

            childList.Add(navItem);
        }
        return childList;
    }

    public async Task<List<UserToRestrictedMenu>> createScalerListUserToRestrictedMenu(UserToRestrictedMenuPostDto model)
    {
        List<UserToRestrictedMenu> utrmList = new List<UserToRestrictedMenu>();
        var menus = model.MenuItems;
        foreach (var item in menus)
        {
            UserToRestrictedMenu utrm = new UserToRestrictedMenu();
            utrm.USER_ID_FK = model.UserId;
            utrm.MENU_ID_FK = item.ItemId;
            utrm.TIMESTAMP = DateTime.Now;
            if (item.Children != null)
            {
                if (item.Children.Count > 0)
                {
                    UserToRestrictedMenuPostDto child = new();
                    child.UserId = model.UserId;
                    child.MenuItems = item.Children;
                    utrmList.AddRange(await createScalerListUserToRestrictedMenu(child));
                    int activeCount = item.Children.Count(x => x.IsActive == (int)BooleanEnum.TRUE);
                    utrm.IS_ACTIVE = activeCount == item.Children.Count ? (int)BooleanEnum.TRUE : (int)BooleanEnum.FALSE;
                }
                else
                {
                    utrm.IS_ACTIVE = item.IsActive;
                }
            }
            else
            {
                utrm.IS_ACTIVE = item.IsActive;
            }

            utrmList.Add(utrm);

        }
        return utrmList;
    }
}