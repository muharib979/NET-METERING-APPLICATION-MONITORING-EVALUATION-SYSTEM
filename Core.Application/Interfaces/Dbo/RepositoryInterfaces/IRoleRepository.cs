using Core.Application.Commands.Dbo.RoleEdit;
using Shared.DTOs.Dbo;

namespace Core.Application.Interfaces.Dbo.RepositoryInterfaces;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<List<DropdownResult>> GetAllRoleDDAsync();
    //Task<int> AddAsync();
    Task<List<RoleDto>> GetAllWithMenuNameAsync(PaginationParams pParams);
    //Task<int> EditRoleAsync(RoleEditCommand request);

    Task<int> EditRoleAsync(RoleEditCommand request)
    {
        var role = new Role
        {
            ID = request.Id,
            IS_ACTIVE = request.IsActive,
            //MENU_ID_FK = request.MenuFkId,
            ROLE_NAME = request.RoleName,
            PRIORITY=request.Priority,

        };
        return UpdateAsync(role);
    }
    Task<int> DeleteRoleAsync(int id);
    Task<List<RoleDto>> GetRoleByUserName(string userName);
    Task<List<RoleDto>> GetRoleForAccessMappingByUser(string userName);


}
