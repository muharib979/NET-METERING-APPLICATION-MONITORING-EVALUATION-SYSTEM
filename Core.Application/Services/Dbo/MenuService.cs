using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;
using Newtonsoft.Json;

namespace Core.Application.Services.Dbo;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public MenuService(IMenuRepository repository, IMapper mapper, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<int> AddAsync(MenuDto entity)
    {
        if (!string.IsNullOrEmpty(entity.Icon) && !string.IsNullOrEmpty(entity.IconSVG))
        {
            var result = await SaveSVGIcon(entity.Icon, entity.IconSVG);

            if (result == 0) return 0;
        }

        return await _repository.AddAsync(_mapper.Map<Menu>(entity));
    }

    public async Task<int> UpdateAsync(MenuDto entity)
    {
        if (!string.IsNullOrEmpty(entity.Icon) && !string.IsNullOrEmpty(entity.IconSVG))
        {
            var result = await SaveSVGIcon(entity.Icon, entity.IconSVG);

            if (result == 0) return 0;
        }

        return await _repository.UpdateAsync(_mapper.Map<Menu>(entity));
    }

    public async Task<MenuDto> GetByIdAsync(int id) => _mapper.Map<MenuDto>(await _repository.GetByIdAsync(id));

    public async Task<List<SideBarItemDto>> GetAllGroupMenu() => _mapper.Map<List<SideBarItemDto>>(await _repository.GetAllMenu());

    public async Task<List<MenuDto>> GetAllAsync(PaginationParams pParams) => _mapper.Map<List<MenuDto>>(await _repository.GetAllAsync(pParams));

    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public async Task<List<DropdownResult>> GetAllDashBoardMenuDDAsync()
    {
        var menusHasnotChild = await _repository.GetAllDashBoardMenuDDAsync();
        var menusHasChild = await _repository.GetAllParentMenuDDAsync();

        var footerMenu = menusHasnotChild.FirstOrDefault(x => x.MENU_NAME == "New Alarm");
        if (footerMenu != null)
        {
            menusHasnotChild.RemoveAll(x => x.GROUP_ID == footerMenu.GROUP_ID);
        }

        foreach (var menu in menusHasnotChild)
        {
            if (menu.PARENT_ID != null)
            {
                if (menu.PARENT_ID != 0)
                {
                    foreach (var item in menusHasChild)
                    {
                        if (item.Key == menu.PARENT_ID)
                        {
                            menu.MENU_NAME = $"{item.Value} - {menu.MENU_NAME}";
                        }
                    }

                }
            }
        }

        var dDres = new List<DropdownResult>();

        foreach (var item in menusHasnotChild)
        {
            dDres.Add(new DropdownResult { Key = item.ID, Value = item.MENU_NAME });
        }

        return dDres;
    }

    public async Task<List<DropdownResult>> GetAllParentMenuDDAsync() => await _repository.GetAllParentMenuDDAsync();

    public async Task<List<DropdownResult>> GetGroupDDAsync() => await _repository.GetGroupDDAsync();


    public async Task<int> GetTotalCountAsync(string searchBy) => await _repository.GetTotalCountAsync(searchBy);


    public async Task<List<SideBarItemDto>> GetAllDashBoardMenu(int roleId) => _mapper.Map<List<SideBarItemDto>>(await _repository.GetAllDashBoardMenu(roleId));

    public async Task<List<SVGICon>> GetSVGIconList()
    {
        var filePath = $"Uploades/SVGIcons/iconsvg.json";

        if (!File.Exists(filePath))
            return new List<SVGICon>();
        //return new BadRequestObjectResult(new { Message = "Can not find svg icons" });

        string json = await File.ReadAllTextAsync(filePath);
        var DataOfSvgs = JsonConvert.DeserializeObject<List<SVGICon>>(json) ?? new List<SVGICon>();


        return DataOfSvgs;
    }

    private async Task<int> SaveSVGIcon(string icon, string svg)
    {
        var status = 0;
        var filePath = $"Uploades/SVGIcons/iconsvg.json";

        if (!File.Exists(filePath))
            return status;

        string json = await File.ReadAllTextAsync(filePath);
        var DataOfSvgs = JsonConvert.DeserializeObject<List<SVGICon>>(json) ?? new List<SVGICon>();

        if (DataOfSvgs.Count > 0) DataOfSvgs.RemoveAll(x => x.Name == icon);


        DataOfSvgs.Add(new SVGICon
        {
            Name = icon,
            Value = svg
        });
        string updatedJson = JsonConvert.SerializeObject(DataOfSvgs, Formatting.Indented);
        await File.WriteAllTextAsync(filePath, updatedJson);

        status = 1;

        return status;
    }

    public Task<int> AddListAsync(List<MenuDto> entity)
    {
        throw new NotImplementedException();
    }
}
