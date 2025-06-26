using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Enums;

namespace Core.Application.Commands.Dbo.MenuRestrictionToUser;

public class MenuRestrictionToUserCommand : UserToRestrictedMenuPostDto, IRequest<Response<IActionResult>> 
{

    public class Handler : IRequestHandler<MenuRestrictionToUserCommand, Response<IActionResult>>
    {
        private readonly IUserToMenuRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IUserToMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IActionResult>> Handle(MenuRestrictionToUserCommand request, CancellationToken cancellationToken)
        {
            List<UserToRestrictedMenu> utrmList = new List<UserToRestrictedMenu>();
            utrmList = await createScalerListUserToRestrictedMenu(request);
            var status = await AssignUserToRestrictedMenu(utrmList);

            return status == 1 ? Response<IActionResult>.Success("User to restricted menu mapping created successfully!") : Response<IActionResult>.Fail("Error Occured!");
        }
        public async Task<int> AssignUserToRestrictedMenu(List<UserToRestrictedMenu> utrmList)
        {
            int status = 0;
            if (utrmList == null)
                return status;

            status = await _repository.AssignUserToRestrictedMenu(utrmList);

            return status;
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

}
