using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.UserAdd;

public class UserAddCommandHandler : IRequestHandler<UserAddCommand, Response<IActionResult>>
{
    private readonly IUserService _service;

    public UserAddCommandHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Response<IActionResult>> Handle(UserAddCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.AddUserAsync(request);

        return result > 0 ? Response<IActionResult>.Success("User Successfully Created") : Response<IActionResult>.Fail("Problem saving changes");
    }
}
