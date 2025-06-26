using Core.Application.Commands.PaymentGateway;
using Core.Application.Interfaces.PaymentGateway;
using Core.Domain.PaymnetGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Core.Application.Commands.NagadPaymentGateway
{
    public class NagadGatewayCommand: IRequest<bool>
    {
        public RequestData RequestData { get; set; }
        public string?  orderId { get; set; }
        public class Handler: IRequestHandler<NagadGatewayCommand, bool>
        {
            private readonly IPaymentGatewayRepository _repository;

            public Handler(IPaymentGatewayRepository repository) {
                _repository = repository;
            }

            public async Task<bool> Handle(NagadGatewayCommand request, CancellationToken cancellationToken)
            {
                var res = await _repository.SaveNagadRequestData(request.RequestData,request.orderId);
                return res;
            }
        }

    }
}
