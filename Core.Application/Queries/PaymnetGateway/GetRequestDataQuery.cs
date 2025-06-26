using Core.Application.Interfaces.MiscBilling;
using Core.Application.Interfaces.PaymentGateway;
using Core.Application.Queries.MiscBilling;
using Core.Domain.PaymnetGateway;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.PaymnetGateway
{
    public class GetRequestDataQuery : IRequest<List<RequestData>>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public class Handler : IRequestHandler<GetRequestDataQuery, List<RequestData>>
        {
            private readonly IPaymentGatewayRepository _repository;

            public Handler(IPaymentGatewayRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<RequestData>> Handle(GetRequestDataQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetBillInformation(request.customerNumber, request.billNumber);

                return result;

            }
        }
    }
}

