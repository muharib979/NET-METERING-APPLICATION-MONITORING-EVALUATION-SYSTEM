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
    public class MiscBillPaymentStutusQuery : IRequest<BillPaymentMiscStatusDto>
    {
        public string BillNumber { get; set; }
        //public string LocationCode { get; set; }

        public class Handler : IRequestHandler<MiscBillPaymentStutusQuery, BillPaymentMiscStatusDto>
        {
            private readonly IBillPaymentMiscConfirmationRepository _repository;
            public Handler(IBillPaymentMiscConfirmationRepository repository)
            {
                _repository = repository;
            }

            public async Task<BillPaymentMiscStatusDto> Handle(MiscBillPaymentStutusQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.BillPaymentMiscStatus(request.BillNumber);
                return result;
            }
        }
    }
}
