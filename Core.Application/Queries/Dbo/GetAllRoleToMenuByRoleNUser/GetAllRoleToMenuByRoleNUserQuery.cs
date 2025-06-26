using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.GetAllRoleToMenuByRoleNUser;

public class GetAllRoleToMenuByRoleNUserQuery : IRequest<Response<List<SideBarItemDto>>>
{
    public int RoleId { get; set; }
    public int UserId { get; set; }



}
