using Shared.DTOs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Dbo.RoleDDList
{
    public class GetRoleByUserNameQuery:IRequest<List<RoleDto>>
    {
        public string UserName { get; set; }

        public class Handler:IRequestHandler<GetRoleByUserNameQuery,List<RoleDto>> {

            private readonly IRoleRepository _repository;
            public Handler(IRoleRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<RoleDto>> Handle(GetRoleByUserNameQuery request, CancellationToken cancellationToken)
            {
                var result=await _repository.GetRoleByUserName(request.UserName);
                return result;
            }
        }
    }
}
