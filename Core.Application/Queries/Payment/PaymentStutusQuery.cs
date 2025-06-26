using Core.Application.Interfaces.Consumer;
using Core.Application.Interfaces.PaymentConfirmation;
using Core.Application.Queries.Consumer;
using Shared.DTOs.CustomerDto;
using Shared.DTOs.PaymentConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Payment
{
    public class PaymentStutusQuery : IRequest<PaymentStatusDto>
    {
        public string BillNumber { get; set; }
        public string LocationCode { get; set; }

        public class Handler : IRequestHandler<PaymentStutusQuery, PaymentStatusDto>
        {
            private readonly IPaymentConfirmationRepository _repository;
            public Handler(IPaymentConfirmationRepository repository)
            {
                _repository = repository;
            }

            public async Task<PaymentStatusDto> Handle(PaymentStutusQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.PaymentStatus(request.BillNumber,request.LocationCode);
                return result;
            }
        }
    }
}
