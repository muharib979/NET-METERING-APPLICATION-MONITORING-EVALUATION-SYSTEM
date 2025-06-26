using Core.Application.Interfaces.Ministry;
using Core.Application.Interfaces.MinistryCustomer;
using Core.Application.Queries.Report.Ministry.GetAllMinistry;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetPouroshovaQueryHandler:IRequest<Response<List<DropdownResultForStringKey>>>
    {
        public class Handler : IRequestHandler<GetPouroshovaQueryHandler, Response<List<DropdownResultForStringKey>>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<Response<List<DropdownResultForStringKey>>> Handle(GetPouroshovaQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllPouroshova();
                return Response<List<DropdownResultForStringKey>>.Success(result, "Success");

            }
        }
    }
}
