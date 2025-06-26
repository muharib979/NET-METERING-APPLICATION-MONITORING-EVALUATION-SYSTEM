using Core.Application.Interfaces.MiscBilling;
using MediatR;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL
{
    public class PenaltyBillPrepaidCommand: PenaltyBillPrepaidDTO, IRequest<ReturnDTO>
    {
        public class Handler: IRequestHandler<PenaltyBillPrepaidCommand, ReturnDTO>
        {
            private readonly IPenaltyBillGenerateRepository _repository;

            public Handler(IPenaltyBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<ReturnDTO> Handle(PenaltyBillPrepaidCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SavePenaltyBillPrepaid(request);
                return result;
            }
        }
          
    }
}
