using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.Customer_Details
{
    public class GetCustomerInfoQuery : IRequest<UserInfoDto>
    {
        public string? customerNumber { get; set; }
        public string? dbCode { get; set; }
        public string? locCode { get; set; }

        public class Handler : IRequestHandler<GetCustomerInfoQuery, UserInfoDto>
        {
            private readonly ICustomerDetailsRepository _repository;

            public Handler(ICustomerDetailsRepository repository)
            {
                _repository = repository;
            }

            public async Task<UserInfoDto> Handle(GetCustomerInfoQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetCustomersInformation(request.customerNumber, request.dbCode, request.locCode);
                return result;

            }
        }
    }
}
