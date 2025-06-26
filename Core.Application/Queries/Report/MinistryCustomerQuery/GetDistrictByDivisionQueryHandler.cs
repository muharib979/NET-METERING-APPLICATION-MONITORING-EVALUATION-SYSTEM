using Core.Application.Interfaces.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetDistrictByDivisionQueryHandler : IRequest<List<DropdownResultForStringKey>>
    {
        public string divisionCode { get; set; }    
        public class Handler : IRequestHandler<GetDistrictByDivisionQueryHandler, List<DropdownResultForStringKey>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetDistrictByDivisionQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllDistrict(request.divisionCode);
                return result;

            }
        }
    }
}
