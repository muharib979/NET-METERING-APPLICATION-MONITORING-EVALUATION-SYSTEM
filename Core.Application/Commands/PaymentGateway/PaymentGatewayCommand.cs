using Core.Application.Interfaces;
using Core.Application.Interfaces.MiscBilling;
using Core.Application.Interfaces.PaymentGateway;
using Core.Application.Queries.MiscBilling;
using Shared.DTOs.MISCBILL;
using Shared.DTOs.PaymentGatewayDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.PaymentGateway
{
    public class PaymentGatewayCommand : IRequest<bool>
    {
        public PaymentReceivedDataDTO command;
        public class Handler : IRequestHandler<PaymentGatewayCommand,bool>
        {
            private readonly IPaymentGatewayRepository _repository;

            public Handler(IPaymentGatewayRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(PaymentGatewayCommand request, CancellationToken cancellationToken)
            {

                var result = await _repository.SavePaymentGateway(request.command);

                return result;

            }
        }
    }
}