using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.UserCheckByUserName;

public class UserCheckByUserNameQuery : IRequest<Response<IActionResult>>
{
    public string UserName { get; set; }
}
