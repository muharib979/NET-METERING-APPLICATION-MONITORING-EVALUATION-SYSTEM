using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.MenuGetById;

public class MenuGetByIdQuery : IRequest<Response<MenuDto>>
{
    public int Id { get; set; }

    public class Handler : IRequestHandler<MenuGetByIdQuery, Response<MenuDto>>
    {
        private readonly IMenuRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IMenuRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Response<MenuDto>> Handle(MenuGetByIdQuery request, CancellationToken cancellationToken)
        {
            var menu = _mapper.Map<MenuDto>(await _repository.GetByIdAsync(request.Id));
            if (menu == null) return Response<MenuDto>.Fail("Menu does not exists");
            return Response<MenuDto>.Success(menu, "Successfully Retrived Menu By Id");
        }
    }
}
