using Core.Application.Interfaces.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetUpozilaByDistrictQueryHandler : IRequest<List<DropdownResultForStringKey>>
    {
        public string districtCode { get; set; }
        public class Handler : IRequestHandler<GetUpozilaByDistrictQueryHandler, List<DropdownResultForStringKey>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<List<DropdownResultForStringKey>> Handle(GetUpozilaByDistrictQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllUpozila(request.districtCode);
                return result;

            }
        }
    }
}