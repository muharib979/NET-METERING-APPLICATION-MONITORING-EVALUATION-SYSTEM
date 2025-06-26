using Core.Application.Interfaces.MinistryCustomer;
using Shared.DTOs.MinistryCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.MinistryCustomerQuery
{
    public class GetMinistryCustomerByIdQuery : IRequest<MinistryCustomerDTOs>
    {
        public string customerNo { get; set; }
        public string locationCode { get; set; }
        public class Handler : IRequestHandler<GetMinistryCustomerByIdQuery, MinistryCustomerDTOs>
        {
            private readonly IMinistryCustomerRepository _ministryRepository;
            public Handler(IMinistryCustomerRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }
            public async Task<MinistryCustomerDTOs> Handle(GetMinistryCustomerByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetMinistryCustomerById(request.customerNo, request.locationCode);
                return result;

            }
        }
    }
}
