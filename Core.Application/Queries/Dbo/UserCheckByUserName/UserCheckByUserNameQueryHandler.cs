using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.UserCheckByUserName;

public class UserCheckByUserNameQueryHandler : IRequestHandler<UserCheckByUserNameQuery, Response<IActionResult>>
{
    private readonly IUserService _service;
    public UserCheckByUserNameQueryHandler(IUserService service)
    {
        _service = service;
    }
    public async Task<Response<IActionResult>> Handle(UserCheckByUserNameQuery request, CancellationToken cancellationToken)
    {

        var user = await _service.GetByNameAsync(request.UserName);
        return user != null ? Response < IActionResult >.Success("User name already exists.") :Response<IActionResult>.Fail("User name not exists.");
    }
}
