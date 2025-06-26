using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.UserEdit;

public class UserEditCommandHandler : IRequestHandler<UserEditCommand, Response<IActionResult>>
{
    private readonly IUserService _service;

    public UserEditCommandHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<IActionResult>> Handle(UserEditCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.UserUpdateAsync(request);

        return result > 0 ? Response<IActionResult>.Success("User Successfully Edited") : Response<IActionResult>.Fail("Problem saving changes");
    }
}
