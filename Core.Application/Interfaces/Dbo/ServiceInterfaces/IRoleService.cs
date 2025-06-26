using Core.Application.Commands.Dbo;
using Core.Application.Commands.Dbo.RoleAdd;
using Core.Application.Commands.Dbo.RoleEdit;

namespace Core.Application.Interfaces.Dbo.ServiceInterfaces;

public interface IRoleService : IBaseService<Role>
{
    Task<List<DropdownResult>> GetAllRoleDDAsync();
    Task<int> AddRoleAsync(RoleAddCommand request);
    Task<int> EditRoleAsync(RoleEditCommand request);
    Task<List<RoleDto>> GetAllWithMenuNameAsync(PaginationParams pParams);
}
