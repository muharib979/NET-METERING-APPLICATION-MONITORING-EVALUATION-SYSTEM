using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.DashboardMenuDDList;

public class DashboardMenuDDListQurey : IRequest<Response<List<DropdownResult>>>
{

    public class Handler : IRequestHandler<DashboardMenuDDListQurey, Response<List<DropdownResult>>>
    {
        private readonly IMenuRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Response<List<DropdownResult>>> Handle(DashboardMenuDDListQurey request, CancellationToken cancellationToken) => Response<List<DropdownResult>>.Success(await GetAllDashBoardMenuDDAsync(), "Successfully Retrived All Dashboard Menu DD List");


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

    }
}
