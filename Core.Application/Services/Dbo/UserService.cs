using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.UserAdd;
using Core.Application.Commands.Dbo.UserAddByCenterLocation;
using Core.Application.Commands.Dbo.UserEdit;
using Core.Application.Common.Mailing;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;
using Core.Domain.Location;
using Shared.DTOs.Enums;
using System.Net;
using System.Security.Cryptography;

namespace Core.Application.Services.Dbo;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;
    private readonly IMailService _mailService;

    public UserService(IUserRepository repository, IConfiguration config, IMailService mailService)
    {
        _repository = repository;
        _config = config;
        _mailService = mailService;
    }

    public Task<int> AddAsync(User entity) => throw new NotImplementedException();

    public async Task<int> AddUserAsync(UserAddCommand model)
    {

        var existUser = await GetByNameAsync(model.UserName);
        if (existUser != null) throw new ArgumentException("User name already exists!");
        var hmac = new HMACSHA512();
        var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_config["SecuritySettings:DefaultPassword"]));

        var user = new User
        {
            FULL_NAME = model.FirstName + " " + model.LastName,
            USER_NAME = model.UserName,
            PASSWORD = Convert.ToBase64String(computerHash),
            PASSWORD_SALT = Convert.ToBase64String(hmac.Key),
            EMAIL = model.Email,
            ENTRY_BY = model.EntryBy,
            ENTRY_DATE = DateTime.Now,
            IS_ACTIVE = (int)BooleanEnum.TRUE,
            IS_DELETED = (int)BooleanEnum.FALSE,
        };
        var mailRequest = new MailRequest()
        {
            To = user.EMAIL,
            UserName = user.USER_NAME,
            TemporaryPassword = _config["SecuritySettings:DefaultPassword"],
            Subject = "Login information for user",
            Body = "User has been created successfully.\n User name : " + user.USER_NAME + ", \n Temporary password: " + _config["SecuritySettings:DefaultPassword"] + ",\n Please change your password through application. Thank You."
        };

        if (mailRequest != null)
        {
            //await _mailService.SendEmail(mailRequest);
        }

        return await _repository.AddUserAsync(user, model.RoleId);
    }

    public async Task<int> UserUpdateAsync(UserEditCommand model) => await _repository.EditUserAsync(model);


    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public Task<List<User>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public async Task<List<UserDto>> GetAllUserAsync(PaginationParams pParams) => await _repository.GetAllUserAsync(pParams);

    public async Task<User> GetByNameAsync(string name) => await _repository.GetByNameAsync(name);

    public async Task<User> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public Task<int> UpdateAsync(User entity) => throw new NotImplementedException();

    public async Task<int> GetTotalCountAsync(string searchBy) => await _repository.GetTotalCountAsync(searchBy);

    public async Task<List<DropdownResult>> GetAllUserDDAsync() => await _repository.GetAllUserDDAsync();

    public async Task<User> GetByEmailAsync(string email) => await _repository.GetByEmailAsync(email);

    public async Task<int> UpdateUserOtp(User user) => await _repository.UpdateUserOtp(user);

    public async Task<User> VerifyOtp(int code, string email) => await _repository.VerifyOtp(code, email);

    public async Task<int> ChangePassword(User user) => await _repository.ChangePassword(user);

    public async Task<Role> GetRoleByUserId(int userId) => await _repository.GetRolesByUserId(userId);
    public async Task<List<string>> GetDbCodeByUserId(int userId) => await _repository.GetDbCodeByUserId(userId);
    public async Task<List<string>> GetLocationCode(int userId) => await _repository.GetLocationCode(userId);

    public Task<int> AddListAsync(List<User> entity)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddUserByCenterLocationAsync(UserAddByCenterLocationCommand model)
    {
        var existUser = await GetByNameAsync(model.UserName);
        if (existUser != null) throw new ArgumentException("User name already exists!");
        var hmac = new HMACSHA512();
        var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.UserPassword));

        var user = new UserCreateByCenterLocation
        {
            USER_NAME = model.UserName,
            PASSWORD = Convert.ToBase64String(computerHash),
            PASSWORD_SALT = Convert.ToBase64String(hmac.Key),
            ENTRY_BY = model.EntryBy,
            ENTRY_DATE = DateTime.Now,
            IS_ACTIVE = (int)BooleanEnum.TRUE,
            IS_DELETED = (int)BooleanEnum.FALSE,
            DB = model.DB,
            Location = model.Location,
        };

        return await _repository.AddUserByCenterLocationAsync(user, model.RoleId);
    }

    public async Task<int> UpdateUserByCenterLocationByAsync(int userId, UserUpdateByCenterLocationCommand model)
    {
        var status = 0;
        var existUser = await GetByNameAsync(model.UserName);
        if (existUser == null) throw new ArgumentException("User Not Found!");
        var user = new UserCreateByCenterLocation
        {
            USER_NAME = model.UserName, 
            DB = model.DB,
            Location = model.Location,
            ROLE_ID = model.RoleId,
        };
        var result = await _repository.UpdateNewUserDatabaseAndLocationMapping(userId, user, model.DB, model.Location);
        if(result == true)
        {
           return status = 1;
        }
        else
        {
            return status;
        }
    }
}