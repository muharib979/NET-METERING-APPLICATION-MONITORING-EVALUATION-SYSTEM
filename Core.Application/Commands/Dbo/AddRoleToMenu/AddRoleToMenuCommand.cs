using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Enums;

namespace Core.Application.Commands.Dbo.AddRoleToMenu;

public class AddRoleToMenuCommand : RoleToMenuPostDto, IRequest<Response<IActionResult>> 
{
    public class Handler : IRequestHandler<AddRoleToMenuCommand, Response<IActionResult>>
    {
        private readonly IRoleToMenuRepository _repository;
        public Handler(IRoleToMenuRepository repository) 
        {
            _repository= repository;
        }
        public async Task<Response<IActionResult>> Handle(AddRoleToMenuCommand request, CancellationToken cancellationToken)
        {
            List<RoleToMenu> rtmList = new List<RoleToMenu>();
            rtmList = await createScalerListRoletToMenu(request);
            var status = await _repository.AssignRoleToMenu(rtmList);

            return status == 1 ? Response<IActionResult>.Success("Role to menu mapping created successfully") : Response<IActionResult>.Fail("Error Occured!");
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
}
