using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Enums;

namespace Core.Application.Commands.Dbo.AddUserToMenu;

public class AddUserToMenuCommand : UserToMenuPostDto, IRequest<Response<IActionResult>>
{
    public class Handler : IRequestHandler<AddUserToMenuCommand, Response<IActionResult>>
    {
        private readonly IUserToMenuRepository _userToMenurepository;
        private readonly IMapper _mapper;

        public Handler(IUserToMenuRepository userToMenurepository, IMapper mapper)
        {
            _userToMenurepository = userToMenurepository;
            _mapper = mapper;
        }

        public async Task<Response<IActionResult>> Handle(AddUserToMenuCommand request, CancellationToken cancellationToken)
        {
            List<UserToMenu> utmList = new List<UserToMenu>();
            utmList = await createScalerListUsertToMenu(request);
            var status = await AssignUserToMenu(utmList);

            return status == 1 ? Response<IActionResult>.Success("User to menu mapping created successfully!") : Response<IActionResult>.Fail("Error Occured!");
        }
        public async Task<int> AssignUserToMenu(List<UserToMenu> utm)
        {
            int status = 0;
            if (utm == null)
                return status;

            status = await _userToMenurepository.AssignUserToMenu(utm);

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
                utm.IS_CREATED = item.IsCreated;
                utm.IS_DELEDTED = item.IsDeleted;
                utm.IS_EDITED = item.IsEdited;
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
    }
}
