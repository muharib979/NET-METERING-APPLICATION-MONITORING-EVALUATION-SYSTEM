using Core.Application.Interfaces.PaymentGateway;
using Core.Domain.PaymnetGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.PaymnetGateway
{
    public class NagadPaymentGateway : IRequest<List<RequestData>>
    {
        public string customerNumber { get; set; }  
        public string billNumber { get; set; }

        public class Handler : IRequestHandler<NagadPaymentGateway, List<RequestData>>
        {
            private readonly IPaymentGatewayRepository _repository;

            public Handler(IPaymentGatewayRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<RequestData>> Handle(NagadPaymentGateway request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetNagadBillInformation(request.customerNumber, request.billNumber);

                return result;

            }
        }
    }
}
