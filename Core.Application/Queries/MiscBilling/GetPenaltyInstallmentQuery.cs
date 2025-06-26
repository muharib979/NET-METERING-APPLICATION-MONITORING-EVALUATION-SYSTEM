using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling;

public class GetPenaltyInstallmentQuery : IRequest<List<InstallmentPlanDTO>>
{
    public string? billNumber { get; set; }
    public class Handler : IRequestHandler<GetPenaltyInstallmentQuery, List<InstallmentPlanDTO>>
    {
        private readonly IInstallmentPlanRepository<InstallmentPlanDTO> _repository;

        public Handler(IInstallmentPlanRepository<InstallmentPlanDTO> repository)
        {
            _repository = repository;
        }

        public async Task<List<InstallmentPlanDTO>> Handle(GetPenaltyInstallmentQuery request, CancellationToken cancellationToken)
        {

            var result = await _repository.GetPenaltyInstallment(request.billNumber);

            return result;

        }
    }
}
    
