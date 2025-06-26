using Shared.DTOs.Common.Pagination;
using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.UserList;

public class UserListQueryHandler : IRequestHandler<UserListQuery, Response<PaginatedList<UserDto>>>
{
    private IUserService _service;

    public UserListQueryHandler(IUserService service) => _service = service;

    public async Task<Response<PaginatedList<UserDto>>> Handle(UserListQuery request, CancellationToken cancellationToken)
    {
        var users = await _service.GetAllUserAsync(request);
        return Response < PaginatedList < UserDto >>.Success(PaginatedList<UserDto>.Create(users, request.PageNumber, request.pageSize, users.Select(u => u.TotalRowCount).FirstOrDefault()), "Successfully Retrived User List");
    }
}
