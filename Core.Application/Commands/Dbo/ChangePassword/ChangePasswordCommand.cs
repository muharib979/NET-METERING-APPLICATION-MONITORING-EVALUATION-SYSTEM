using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.ChangePassword;

public class ChangePasswordCommand : IRequest<Response<IActionResult>>
{
    public string UserName { get; set; }
    public string NewPassword { get; set; }
    public string? CurrentPassword { get; set; }
  
}
