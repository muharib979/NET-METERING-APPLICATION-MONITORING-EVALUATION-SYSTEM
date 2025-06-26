using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class PenaltyBillInstallmentPlanCommand : List<InstallmentPlanDTO>, IRequest<bool>
    {
        public class Handler : IRequestHandler<PenaltyBillInstallmentPlanCommand, bool>
        {
            private readonly IInstallmentPlanRepository<InstallmentPlanDTO> _repository;

            public Handler(IInstallmentPlanRepository<InstallmentPlanDTO> repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(PenaltyBillInstallmentPlanCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SavePenaltyInstallment(request);
                return result;

            }
        }
    }
}

