using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.Customer_Details;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetCustomerByBillNumberQuery : IRequest<CustomerDetailsDTO>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetCustomerByBillNumberQuery, CustomerDetailsDTO>
        {
            private readonly ICustomerDetailsRepository _repository;

            public Handler(ICustomerDetailsRepository repository)
            {
                _repository = repository;
            }

            public async Task<CustomerDetailsDTO> Handle(GetCustomerByBillNumberQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetCustomerByBillNumber(request.billNumber, request.customerNumber);

                return result;

            }
        }
    }
}
