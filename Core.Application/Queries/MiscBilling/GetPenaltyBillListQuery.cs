using Core.Application.Commands.MISCBILL;
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetPenaltyBillListQuery : PenaltyBillViewDTO, IRequest<List<PenaltyBillViewDTO>>
    {
        public class Handler : IRequestHandler<GetPenaltyBillListQuery, List<PenaltyBillViewDTO>>
        {
            private readonly IPenaltyBillGenerateRepository _repository;

            public Handler(IPenaltyBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PenaltyBillViewDTO>> Handle(GetPenaltyBillListQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPenaltyView(request);
                return result;

            }
        }
    }
}
