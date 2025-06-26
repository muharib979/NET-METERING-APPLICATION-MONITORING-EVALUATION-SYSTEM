using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling
{
    public class GetPenaltyBillPrepaidQuery: PenaltyBillPrtepaidViewDTO,IRequest<List<PenaltyBillPrtepaidViewDTO>>
    {
        public class Handler : IRequestHandler<GetPenaltyBillPrepaidQuery, List<PenaltyBillPrtepaidViewDTO>>
        {
            private readonly IPenaltyBillGenerateRepository _repository;

            public Handler(IPenaltyBillGenerateRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PenaltyBillPrtepaidViewDTO>> Handle(GetPenaltyBillPrepaidQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetPenaltyBillPrepaidView(request);
                return result;
            }
        }
        }
}
