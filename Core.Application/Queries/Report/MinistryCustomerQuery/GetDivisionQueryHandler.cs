using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetDivisionQueryHandler : IRequest<List<DropdownResultForStringKey>>
    {
        public class Handler : IRequestHandler<GetDivisionQueryHandler, List<DropdownResultForStringKey>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetDivisionQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllDivision();
                return result;

            }
        }
    }
}

