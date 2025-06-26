using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetMinistryDeptQueryHandler : IRequest<Response<List<DropdownResultForStringKey>>>
    {
        public string ministryCode { get; set; }
        public class Handler : IRequestHandler<GetMinistryDeptQueryHandler, Response<List<DropdownResultForStringKey>>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<Response<List<DropdownResultForStringKey>>> Handle(GetMinistryDeptQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllMinistryDepartment(request.ministryCode);
                return Response<List<DropdownResultForStringKey>>.Success(result, "Success");

            }
        }
    }
}
