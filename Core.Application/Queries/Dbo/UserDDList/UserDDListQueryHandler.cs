using Shared.DTOs.Common.Wrappers;
using System.Collections.Generic;

namespace Core.Application.Queries.Dbo.UserDDList;

public class UserDDListQueryHandler : IRequestHandler<UserDDListQurey, Response<List<DropdownResult>>>
{
    private readonly IUserService _service;

    public UserDDListQueryHandler(IUserService service)
    {
        _service = service;
    }
    public async Task<Response<List<DropdownResult>>> Handle(UserDDListQurey request, CancellationToken cancellationToken) => Response < List < DropdownResult >>.Success(await _service.GetAllUserDDAsync(), "Successfully Retrived All User DD List");
}
