using Core.Application.Interfaces.Dbo.RepositoryInterfaces;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Dbo.DeleteRole
{
    public class DeleteRoleCommand: IRequest<Response<RoleDto>>
    {
        public int id { get; set; }
        public class Handler : IRequestHandler<DeleteRoleCommand, Response<RoleDto>>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;
            public Handler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }

            public async Task<Response<RoleDto>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
            {
                var query = await _roleRepository.DeleteRoleAsync(request.id);
                return Response<RoleDto>.Success("Role Successfully Deleted");
            }
        }
    }
}
