using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.RoleDDList;

public class RoleDDListQurey : IRequest<Response<List<DropdownResult>>> 
{
    public class Handler : IRequestHandler<RoleDDListQurey, Response<List<DropdownResult>>>
    {
        private readonly IRoleRepository _repository;
        public Handler(IRoleRepository repository) 
        {
            _repository= repository;
        }
        public async Task<Response<List<DropdownResult>>> Handle(RoleDDListQurey request, CancellationToken cancellationToken)
        {
            return Response<List<DropdownResult>>.Success(await _repository.GetAllRoleDDAsync(), "Successfully Retrived Role DD List");
        }
    }
}
