using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.BillingReason
{
    public class GetBillReasonMiscChargeBillQuery : IRequest<Response<List<BillReasonDTO>>>
    {
        public class Handler : IRequestHandler<GetBillReasonMiscChargeBillQuery, Response<List<BillReasonDTO>>>
        {
            private readonly IBillingReasonRepository _repository;

            public Handler(IBillingReasonRepository repository)
            {
                _repository = repository;
            }
            public async Task<Response<List<BillReasonDTO>>> Handle(GetBillReasonMiscChargeBillQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetBillReasonForMiscChargeBill();

                return Response<List<BillReasonDTO>>.Success(result, "Success");
            }
        }
    }
}
