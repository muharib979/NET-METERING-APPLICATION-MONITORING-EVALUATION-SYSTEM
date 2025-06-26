using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.RoleAdd;

public class RoleAddCommand : IRequest<Response<IActionResult>>
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public int MenuFkId { get; set; }
    public int IsActive { get; set; }
    public int Priority { get; set; }

    public class Handler : IRequestHandler<RoleAddCommand, Response<IActionResult>>
    {
        private readonly IRoleRepository _repository;
        public Handler(IRoleRepository repository) 
        {
            _repository= repository;
        }
        public async Task<Response<IActionResult>> Handle(RoleAddCommand request, CancellationToken cancellationToken)
        {
                    var role = new Role
                    {
                        IS_ACTIVE = request.IsActive,
                        MENU_ID_FK = (int)request.MenuFkId,
                        ROLE_NAME = request.RoleName,
                        PRIORITY= request.Priority,
                    };
            int result = await _repository.AddAsync(role);

            return result > 0 ? Response<IActionResult>.Success("Role Successfully Created") : Response<IActionResult>.Fail("Problem saving changes");
        }
    }
}
