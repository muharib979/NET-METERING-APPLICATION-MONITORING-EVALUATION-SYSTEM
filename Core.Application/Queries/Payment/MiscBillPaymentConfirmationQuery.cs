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
    public class MiscBillPaymentConfirmationQuery : IRequest<BillPaymentMiscConfirmationDto>
    {
        public string BillNumber { get; set; }
        //public string LocationCode { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public class Handler : IRequestHandler<MiscBillPaymentConfirmationQuery, BillPaymentMiscConfirmationDto>
        {
            private readonly IBillPaymentMiscConfirmationRepository _repository;
            public Handler(IBillPaymentMiscConfirmationRepository repository)
            {
                _repository = repository;
            }

            public async Task<BillPaymentMiscConfirmationDto> Handle(MiscBillPaymentConfirmationQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.BillPaymentMiscConfirmation(request.BillNumber,request.IsPaid,request.PaymentDate);
                return result;
            }
        }
    }
}
