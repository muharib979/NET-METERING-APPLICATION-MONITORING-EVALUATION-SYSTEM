using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Queries.Dbo.UserList;

public class UserListQuery : PaginationParams, IRequest<Response<PaginatedList<UserDto>>> { }

