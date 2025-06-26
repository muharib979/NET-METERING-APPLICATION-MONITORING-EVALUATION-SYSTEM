using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.BillingReason
{
    public class GetPenaltyBillNonCustQuery : IRequest<List<BillReasonDTO>>
    {
        public class Handler : IRequestHandler<GetPenaltyBillNonCustQuery, List<BillReasonDTO>>
        {
            private readonly IBillingReasonRepository _repository;

            public Handler(IBillingReasonRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<BillReasonDTO>> Handle(GetPenaltyBillNonCustQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetBillReasonForPenaltyNonCust();

                return (result);

            }
        }
    }
}
