using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.RoleList;

public class RoleListQuery : PaginationParams, IRequest<Response<PaginatedList<RoleDto>>> 
{
    public class Handler : IRequestHandler<RoleListQuery, Response<PaginatedList<RoleDto>>>
    {
        private readonly IRoleRepository _repository;
        public Handler(IRoleRepository repository) 
        {
            _repository = repository;
        }
        public async Task<Response<PaginatedList<RoleDto>>> Handle(RoleListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _repository.GetAllWithMenuNameAsync(request);

            return Response<PaginatedList<RoleDto>>.Success(PaginatedList<RoleDto>.Create(roles, request.PageNumber, request.pageSize, roles.Select(r => r.TotalRowCount).FirstOrDefault()), "Successfully Retrived Role List");
        }
    }
}

