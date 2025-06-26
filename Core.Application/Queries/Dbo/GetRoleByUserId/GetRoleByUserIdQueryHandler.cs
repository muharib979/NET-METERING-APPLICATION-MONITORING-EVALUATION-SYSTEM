using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.GetRoleByUserId;

public class GetRoleByUserIdQueryHandler : IRequestHandler<GetRoleByUserIdQuery, Response<RoleDto>>
{
    private readonly IUserService _service;
    private readonly IMapper _mapper;

    public GetRoleByUserIdQueryHandler(IUserService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<Response<RoleDto>> Handle(GetRoleByUserIdQuery request, CancellationToken cancellationToken)
    {
        var role = _mapper.Map<RoleDto>(await _service.GetRoleByUserId(request.Id));

        return role != null ? Response < RoleDto >.Success(role, "Successfully Retrived Role By User Id") : Response<RoleDto>.Fail("Role is not found");

    }
}
