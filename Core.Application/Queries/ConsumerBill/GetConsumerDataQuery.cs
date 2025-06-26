using Core.Application.Interfaces.ConsumerBill;
using Core.Application.Interfaces.PaymentGateway;
using Core.Domain.PaymnetGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.ConsumerBill
{
    public class GetConsumerDataQuery : IRequest<List<Domain.MISCBILL.ConsumerBill>>
    {
        public string? customerNumber { get; set; }
        public string? billNumber { get; set; }
        public class Handler : IRequestHandler<GetConsumerDataQuery, List<Domain.MISCBILL.ConsumerBill>>
        {
            private readonly IConsumerBillRepository _repository;

            public Handler(IConsumerBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<Domain.MISCBILL.ConsumerBill>> Handle(GetConsumerDataQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetConsumerData(request.customerNumber, request.billNumber);

                return result;

            }
        }
    }
}
