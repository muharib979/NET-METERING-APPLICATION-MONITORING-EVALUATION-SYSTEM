using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.GetRoleByUserId;

public class GetRoleByUserIdQuery : IRequest<Response<RoleDto>>
{
    public int Id { get; set; }
}
