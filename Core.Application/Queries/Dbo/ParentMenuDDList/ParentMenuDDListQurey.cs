using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.ParentMenuDDList;

public class ParentMenuDDListQurey : IRequest<Response<List<DropdownResult>>> 
{
    public class Handler : IRequestHandler<ParentMenuDDListQurey, Response<List<DropdownResult>>>
    {
        private readonly IMenuRepository _menurepository;
        private readonly IMapper _mapper;

        public Handler(IMenuRepository repository, IMapper mapper)
        {
            _menurepository = repository;
            _mapper = mapper;
        }
        public async Task<Response<List<DropdownResult>>> Handle(ParentMenuDDListQurey request, CancellationToken cancellationToken) => Response<List<DropdownResult>>.Success(await _menurepository.GetAllParentMenuDDAsync(), "Successfully Retrived Parent Menu DD List");
    }
}

