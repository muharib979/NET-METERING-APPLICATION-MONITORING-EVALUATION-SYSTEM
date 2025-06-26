using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class BillDetailsByCustomerQuery : IRequest<List<CustomerBillDetailsDTO>>
    {
        public string? customerNumber { get; set; }
        public string? locationCode { get; set; }

      
        public class Handler : IRequestHandler<BillDetailsByCustomerQuery, List<CustomerBillDetailsDTO>>
        {
            private readonly IBillDetailsRepository<CustomerBillDetailsDTO> _repository;

            public Handler(IBillDetailsRepository<CustomerBillDetailsDTO> repository)
            {
                _repository = repository;
            }

            public async Task<List<CustomerBillDetailsDTO>> Handle(BillDetailsByCustomerQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.BillDetailsByCustomer(request.customerNumber,request.locationCode);

                return result;

            }
        }
    }
}
