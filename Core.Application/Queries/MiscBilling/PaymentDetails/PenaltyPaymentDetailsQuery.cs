using Core.Application.Interfaces.MiscBilling;
using MediatR;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.PaymentDetails
{
    public class PenaltyPaymentDetailsQuery:IRequest<List<PaymentDetailsDTO>>
    {
        public class Handler : IRequestHandler<PenaltyPaymentDetailsQuery, List<PaymentDetailsDTO>>
{
            private readonly IPaymentDetailsRepository _repository;

            public Handler(IPaymentDetailsRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PaymentDetailsDTO>> Handle(PenaltyPaymentDetailsQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPenaltyPaymentDetails();
                return result;
            }
        }
    }
}
