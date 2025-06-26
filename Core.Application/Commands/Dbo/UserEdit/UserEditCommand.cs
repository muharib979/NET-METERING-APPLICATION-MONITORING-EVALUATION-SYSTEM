using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.UserEdit;

public class UserEditCommand : IRequest<Response<IActionResult>>
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public string? UpdatedBy { get; set; }
}
