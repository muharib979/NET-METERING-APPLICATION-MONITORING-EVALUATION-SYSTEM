using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Enums;

namespace Core.Application.Queries.Dbo.GetAllMenuByRoleHirerchy;

public class GetAllMenuByRoleHirerchyQuery : IRequest<Response<List<NavItemDto>>>
{
    public int RoleId { get; set; }

    public class Handler : IRequestHandler<GetAllMenuByRoleHirerchyQuery, Response<List<NavItemDto>>>
    {
        private readonly IRoleToMenuRepository _repository;
        private readonly IMapper _mapper;
        public Handler(IRoleToMenuRepository repository, IMapper mapper) 
        {
            _repository= repository;
            _mapper= mapper;
        }
        public async Task<Response<List<NavItemDto>>> Handle(GetAllMenuByRoleHirerchyQuery request, CancellationToken cancellationToken)
        {
            var result = await GetAllMenuHirerchyForRole(request.RoleId);

            return Response<List<NavItemDto>>.Success(result, "Successfully Retrived All Menu Hirerchy For Role");
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
    }
}
