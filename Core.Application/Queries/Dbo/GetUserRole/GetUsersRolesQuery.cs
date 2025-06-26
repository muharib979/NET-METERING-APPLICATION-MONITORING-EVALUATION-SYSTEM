using Core.Application.Queries.MiscBilling.BillingReason;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Dbo.GetUserRole;

    public class GetUsersRolesQuery : IRequest<Response<List<UserRole>>>
{
        public class Handler : IRequestHandler<GetUsersRolesQuery, Response<List<UserRole>>>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;

            }
            public async Task<Response<List<UserRole>>> Handle(GetUsersRolesQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetUsersRolesAsync();

                return Response<List<UserRole>>.Success(result, "Success");
            
            }
        }

}

