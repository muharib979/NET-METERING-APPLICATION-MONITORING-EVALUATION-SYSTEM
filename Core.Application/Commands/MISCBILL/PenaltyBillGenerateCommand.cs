using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.MiscBilling.CalculateBillFor;
using Core.Domain.MISCBILL;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class PenaltyBillGenerateCommand:PenaltyBillDTO,IRequest<ReturnDTO>
    {
        public class Handler : IRequestHandler<PenaltyBillGenerateCommand, ReturnDTO>
        {
            private readonly IPenaltyBillGenerateRepository _repository;

            public Handler(IPenaltyBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<ReturnDTO> Handle(PenaltyBillGenerateCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SavePenaltyBill(request);
                return result;

            }
        }
    }
}
