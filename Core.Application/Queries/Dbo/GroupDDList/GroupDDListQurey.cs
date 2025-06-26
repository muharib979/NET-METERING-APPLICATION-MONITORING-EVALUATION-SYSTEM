using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.GroupDDList;

public class GroupDDListQurey : IRequest<Response<List<DropdownResult>>> 
{
    public class Handler : IRequestHandler<GroupDDListQurey, Response<List<DropdownResult>>>
    {
        private readonly IMenuRepository _menurepository;
        private readonly IMapper _mapper;

        public Handler(IMenuRepository repository, IMapper mapper)
        {
            _menurepository = repository;
            _mapper = mapper;
        }
        public async Task<Response<List<DropdownResult>>> Handle(GroupDDListQurey request, CancellationToken cancellationToken) => Response<List<DropdownResult>>.Success(await _menurepository.GetGroupDDAsync(), "Successfully Retrived All Group DropDown");
    }

}
