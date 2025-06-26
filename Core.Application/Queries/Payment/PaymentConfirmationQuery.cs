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
    public class PaymentConfirmationQuery : IRequest<PaymentConfirmationDto>
    {
        public string BillNumber { get; set; }
        public string LocationCode { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public class Handler : IRequestHandler<PaymentConfirmationQuery, PaymentConfirmationDto>
        {
            private readonly IPaymentConfirmationRepository _repository;
            public Handler(IPaymentConfirmationRepository repository)
            {
                _repository = repository;
            }

            public async Task<PaymentConfirmationDto> Handle(PaymentConfirmationQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.PaymentConfirmation(request.BillNumber,request.LocationCode,request.IsPaid,request.PaymentDate);
                return result;
            }
        }
    }
}
