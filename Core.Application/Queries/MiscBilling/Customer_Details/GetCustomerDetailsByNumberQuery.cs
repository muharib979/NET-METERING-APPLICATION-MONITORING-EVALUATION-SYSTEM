using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.Imposed_By;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Customer_Details
{
    public class GetCustomerDetailsByNumberQuery : IRequest<CustomerDetailsDTO>
    {
        public string? customerNumber { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetCustomerDetailsByNumberQuery, CustomerDetailsDTO>
        {
            private readonly ICustomerDetailsRepository _repository;

            public Handler(ICustomerDetailsRepository repository)
            {
                _repository = repository;
            }

            public async Task<CustomerDetailsDTO> Handle(GetCustomerDetailsByNumberQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetCustomerByCustomerNymber(request.customerNumber,request.locationCode);

                return result;

            }
        }
    }
}
