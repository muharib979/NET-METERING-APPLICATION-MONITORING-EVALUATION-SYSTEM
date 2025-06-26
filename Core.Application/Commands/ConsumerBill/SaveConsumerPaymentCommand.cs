using Core.Application.Commands.PaymentGateway;
using Core.Application.Interfaces.ConsumerBill;
using Core.Application.Interfaces.PaymentGateway;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.ConsumerBill
{
    public class SaveConsumerPaymentCommand : IRequest<bool>
    {
        public PaymentReceivedDataDTO command;
        public class Handler : IRequestHandler<SaveConsumerPaymentCommand, bool>
        {
            private readonly IConsumerBillRepository _repository;

            public Handler(IConsumerBillRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveConsumerPaymentCommand request, CancellationToken cancellationToken)
            {

                var result = await _repository.SaveConsumerPayment(request.command);

                return result;

            }
        }
    }
}
