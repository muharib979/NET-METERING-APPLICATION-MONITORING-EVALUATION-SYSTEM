using Core.Application.Interfaces.Religious;
using Shared.DTOs.Religious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Religious
{
    public class GetReligiousArrearSummaryQueryHandler:IRequest<List<ReligiousArrearSummaryDTO>>
    {
        public class Handler : IRequestHandler<GetReligiousArrearSummaryQueryHandler, List<ReligiousArrearSummaryDTO>>
        {
            private readonly IReligiousRepository _religiousRepository;
            public Handler(IReligiousRepository religiousRepository)
            {
                _religiousRepository= religiousRepository;
            }

            public async Task<List<ReligiousArrearSummaryDTO>> Handle(GetReligiousArrearSummaryQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _religiousRepository.GetReligiousArrearSummary();
                return result;
            }
        }
    }
}
