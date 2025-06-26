using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.CustomerInstallment
{
    public class GetPenaltyBillInstallmentPlanQuery : IRequest<List<PenaltyBillViewDTO>>
    {
        public string? billNumber { get; set; }
        public class Handler : IRequestHandler<GetPenaltyBillInstallmentPlanQuery, List<PenaltyBillViewDTO>>
        {
            private readonly ICustomerInstallmentRepository _repository;

            public Handler(ICustomerInstallmentRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PenaltyBillViewDTO>> Handle(GetPenaltyBillInstallmentPlanQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetPenaltyBillInstallmentPlan(request.billNumber);

                return result;

            }
        }
    }
}
