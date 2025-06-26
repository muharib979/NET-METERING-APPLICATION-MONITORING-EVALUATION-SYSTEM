using Shared.DTOs.Common.Wrappers;

namespace Core.Application.Commands.Dbo.RoleEdit;

public class RoleEditCommand : IRequest<Response<RoleDto>>
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    //public int? MenuFkId { get; set; }
    public int IsActive { get; set; } // bool
    public int Priority { get; set; } 

    public class Handler : IRequestHandler<RoleEditCommand, Response<RoleDto>>
    {
        private readonly IRoleRepository _repository;
        public Handler(IRoleRepository repository) 
        {
            _repository= repository;
        }

        public async Task<Response<RoleDto>> Handle(RoleEditCommand request, CancellationToken cancellationToken)
        {
            int result = await _repository.EditRoleAsync(request);
            return result > 0 ? Response<RoleDto>.Success("Role Successfully Edited") : Response<RoleDto>.Fail("Problem saving changes");
        }
        //public async Task<Response<IActionResult>> Handle(RoleEditCommand request, CancellationToken cancellationToken)
        //{
        //    int result = await _repository.EditRoleAsync(request);

        //    return result > 0 ? Response<IActionResult>.Success("Role Successfully Edited") : Response<IActionResult>.Fail("Problem saving changes");
        //}
    }
}
