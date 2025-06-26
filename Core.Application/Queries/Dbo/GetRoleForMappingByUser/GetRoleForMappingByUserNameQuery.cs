using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Dbo.GetRoleForMappingByUser
{
    public class GetRoleForMappingByUserNameQuery: IRequest<List<RoleDto>>
    {
        public string UserName { get; set; }

        public class Handler : IRequestHandler<GetRoleForMappingByUserNameQuery, List<RoleDto>>
        {
            private readonly IRoleRepository _repository;
            public Handler(IRoleRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<RoleDto>> Handle(GetRoleForMappingByUserNameQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetRoleForAccessMappingByUser(request.UserName);
                return result;
            }
        }
    }
}
