using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.Ministry;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetMinistryWiseCustomerCountQuery : IRequest<List<MinistryWiseCustomerCountDTO>>
    {
        public string ministryCode { get; set; }
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetMinistryWiseCustomerCountQuery, List<MinistryWiseCustomerCountDTO>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryCustomerRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }
            public async Task<List<MinistryWiseCustomerCountDTO>> Handle(GetMinistryWiseCustomerCountQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetMinistryWiseCustomerCount(request.ministryCode, request.locationCode);
                return result;
            }
        }
    }
}
