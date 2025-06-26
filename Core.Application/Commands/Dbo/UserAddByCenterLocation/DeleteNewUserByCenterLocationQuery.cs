using Core.Application.Queries.Dbo.UserGetById;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Dbo.UserAddByCenterLocation
{
    public class DeleteNewUserByCenterLocationQuery : IRequest<Response<CreateUserToRoleDto>>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<DeleteNewUserByCenterLocationQuery, Response<CreateUserToRoleDto>>
        {
            private readonly IUserRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUserRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<Response<CreateUserToRoleDto>> Handle(DeleteNewUserByCenterLocationQuery request, CancellationToken cancellationToken)
            {
                var user = await _repository.DeleteNewUserByCenterLocation(request.Id);
                
                return Response<CreateUserToRoleDto>.Success("Successfully Delete User");
            }
        }

    }
}
