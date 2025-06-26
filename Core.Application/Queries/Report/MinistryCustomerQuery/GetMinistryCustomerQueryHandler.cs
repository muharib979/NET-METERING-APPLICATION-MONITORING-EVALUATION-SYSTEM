using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetMinistryCustomerQueryHandler : IRequest<List<MinistryCustomerGetDTOs>>
    {
        public string? customerNo { get; set; }
        public string? locationCode { get; set; }
        public string? centerCode { get; set; }
        public string? ministryCode { get; set; }  
        public string? cityCode { get; set; }  
        public bool? isRebate { get; set; }  
        public class Handler : IRequestHandler<GetMinistryCustomerQueryHandler, List<MinistryCustomerGetDTOs>>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<List<MinistryCustomerGetDTOs>> Handle(GetMinistryCustomerQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllMinistryCustomer(request.customerNo, request.centerCode, request.locationCode, request.ministryCode, request.cityCode, request.isRebate);
                return result;

            }
        }
    }
}
