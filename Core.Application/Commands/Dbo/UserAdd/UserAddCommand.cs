using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.UserAdd;

public class UserAddCommand : IRequest<Response<IActionResult>>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public string? EntryBy { get; set; }
}
