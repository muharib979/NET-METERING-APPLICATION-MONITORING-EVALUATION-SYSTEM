using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.UserAdd;
using Core.Application.Commands.Dbo.UserEdit;
using Core.Application.Queries.Dbo.UserCheckByUserName;

namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<int> AddUserAsync(User entity, int roleId);
    Task<int> AddUserByCenterLocationAsync(UserCreateByCenterLocation entity, int roleId);
    Task<int> EditUserAsync(UserEditCommand model);
    Task<User> GetByNameAsync(string name);
    Task<User> GetByEmailAsync(string email);
    Task<List<DropdownResult>> GetAllUserDDAsync();
    Task<List<DropdownResult>> GetAllUserDdByPriorityAndUser(string userName, string locationCode);
    Task<int> UpdateUserOtp(User entity);
    Task<User> VerifyOtp(int code, string email);
    Task<int> ChangePassword(User entity);
    Task<Role> GetRolesByUserId(int userId);
    Task<List<string>> GetDbCodeByUserId(int userId);
    Task<List<string>> GetLocationCode(int userId);
    Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams);
    Task<List<UserRole>> GetUsersRolesAsync();
    Task<List<GetUserCreateByCenterLocationModel>> GetNewUserByCenterLocationById(int userId);
    Task AddUserAsync(UserAddCommand request);
    Task<int> DeleteNewUserByCenterLocation(int id);
    Task<bool> UpdateNewUserDatabaseAndLocationMapping(int UserId, UserCreateByCenterLocation user, List<int> dbs, List<int> locations);

}
