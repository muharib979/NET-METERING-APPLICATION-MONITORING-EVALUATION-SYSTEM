﻿using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Enums;

namespace Core.Application.Queries.Dbo.GetAllMenuByUserHirerchy;

public class GetAllMenuByUserHirerchyQuery : IRequest<Response<List<NavItemDto>>>
{
    public int UserId { get; set; }


    public class Handler : IRequestHandler<GetAllMenuByUserHirerchyQuery, Response<List<NavItemDto>>>
    {
        private readonly IUserToMenuRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IUserToMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<List<NavItemDto>>> Handle(GetAllMenuByUserHirerchyQuery request, CancellationToken cancellationToken)
        {
            var result = await GetAllMenuHirerchyForUser(request.UserId);
            return Response<List<NavItemDto>>.Success(result, "Successfully Retrived All Menu Hirerchy For User");
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
    }

}
