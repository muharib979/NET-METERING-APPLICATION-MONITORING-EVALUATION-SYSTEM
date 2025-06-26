using AutoMapper;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.MenuList;

public class MenuListQuery : PaginationParams, IRequest<Response<PaginatedList<MenuDto>>> 
{


    public class Handler : IRequestHandler<MenuListQuery, Response<PaginatedList<MenuDto>>>
    {
        private readonly IMenuRepository _repository;
        private readonly IMapper _mapper;
        public Handler(IMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Response<PaginatedList<MenuDto>>> Handle(MenuListQuery request, CancellationToken cancellationToken)
        {
            var menus = _mapper.Map<List<MenuDto>>(await _repository.GetAllAsync(request));


            return Response<PaginatedList<MenuDto>>.Success(PaginatedList<MenuDto>.Create(menus, request.PageNumber, request.pageSize, menus.Select(m => m.TotalRowCount).FirstOrDefault()), "Successfully Retrived Menu List");
        }
    }
}

