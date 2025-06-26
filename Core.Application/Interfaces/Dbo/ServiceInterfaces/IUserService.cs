using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.UserAdd;
using Core.Application.Commands.Dbo.UserAddByCenterLocation;
using Core.Application.Commands.Dbo.UserEdit;

namespace Core.Application.Interfaces.Dbo.ServiceInterfaces;

public interface IUserService : IBaseService<User>
{
    Task<int> AddUserAsync(UserAddCommand model);
    Task<int> AddUserByCenterLocationAsync(UserAddByCenterLocationCommand model);
    Task<int> UserUpdateAsync(UserEditCommand model);
    Task<User> GetByNameAsync(string name);
    Task<User> GetByEmailAsync(string email);
    Task<List<DropdownResult>> GetAllUserDDAsync();
    Task<int> UpdateUserOtp(User user);
    Task<User> VerifyOtp(int code, string email);
    Task<int> ChangePassword(User user);
    Task<Role> GetRoleByUserId(int userId);
    Task<List<string>> GetDbCodeByUserId(int userId);
    Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams);
    Task<int> UpdateUserByCenterLocationByAsync(int UserId, UserUpdateByCenterLocationCommand model);
    Task<List<string>> GetLocationCode(int userId);

}