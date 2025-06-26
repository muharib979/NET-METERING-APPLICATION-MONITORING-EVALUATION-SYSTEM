using Core.Application.Commands.MISCBILL;
using Core.Application.Interfaces.MiscBilling;
using Core.Application.Interfaces.PaymentGateway;
using Core.Domain.PaymnetGateway;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.PaymentGateway
{
    public class SaveRequestDataCommand : IRequest<bool>
    {
        public RequestData RequestData { get; set; }
        public class Handler : IRequestHandler<SaveRequestDataCommand, bool>
        {
            private readonly IPaymentGatewayRepository _repository;
            public Handler(IPaymentGatewayRepository repository)
            {
                _repository = repository;
            }
            public async Task<bool> Handle(SaveRequestDataCommand request, CancellationToken cancellationToken)
            {
                var res = await _repository.SaveRequestData(request.RequestData);
                return res;
            }
        }
    }
}
