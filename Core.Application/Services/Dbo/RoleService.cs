using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.RoleAdd;
using Core.Application.Commands.Dbo.RoleEdit;
using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Core.Application.Interfaces.Dbo.ServiceInterfaces;

namespace Core.Application.Services.Dbo;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository) => _repository = repository;

    public Task<int> AddAsync(Role entity) => throw new NotImplementedException();

    public async Task<int> AddRoleAsync(RoleAddCommand request)
    {
        var role = new Role
        {
            IS_ACTIVE = request.IsActive,
            //MENU_ID_FK = (int)request.MenuFkId,
            ROLE_NAME = request.RoleName
        };
        return await _repository.AddAsync(role);
    }

    public Task<int> DeleteAsync(int id) => throw new NotImplementedException();

    public async Task<int> EditRoleAsync(RoleEditCommand request)
    {
        var role = new Role
        {
            ID = request.Id,
            IS_ACTIVE = request.IsActive,
            //MENU_ID_FK = request.MenuFkId,
            ROLE_NAME = request.RoleName

        };
        return await _repository.UpdateAsync(role);
    }

    public async Task<List<RoleDto>> GetAllWithMenuNameAsync(PaginationParams pParams) => await _repository.GetAllWithMenuNameAsync(pParams);

    public async Task<List<DropdownResult>> GetAllRoleDDAsync() => await _repository.GetAllRoleDDAsync();

    public async Task<Role> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<int> GetTotalCountAsync(string searchBy) => await _repository.GetTotalCountAsync(searchBy);

    public Task<int> UpdateAsync(Role entity) => throw new NotImplementedException();

    public Task<List<Role>> GetAllAsync(PaginationParams pParams) => throw new NotImplementedException();

    public Task<int> AddListAsync(List<Role> entity)
    {
        throw new NotImplementedException();
    }
}