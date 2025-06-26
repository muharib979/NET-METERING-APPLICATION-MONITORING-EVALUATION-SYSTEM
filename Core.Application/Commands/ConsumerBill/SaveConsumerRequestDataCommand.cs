using Core.Application.Interfaces.ConsumerBill;
using Core.Application.Interfaces.PaymentGateway;
using Core.Domain.PaymnetGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ConsumerBill
{
    public class SaveConsumerRequestDataCommand : IRequest<bool>
    {
        public Domain.MISCBILL.ConsumerBill ConsumerData { get; set; }
        public class Handler : IRequestHandler<SaveConsumerRequestDataCommand, bool>
        {
            private readonly IConsumerBillRepository _repository;
            public Handler(IConsumerBillRepository repository)
            {
                _repository = repository;
            }
            public async Task<bool> Handle(SaveConsumerRequestDataCommand request, CancellationToken cancellationToken)
            {
                var res = await _repository.SaveConsumerRequestData(request.ConsumerData);
                return res;
            }
        }
    }
}
