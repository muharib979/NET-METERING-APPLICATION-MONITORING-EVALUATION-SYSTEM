using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetOnlineMinistryArrearUpToDateQuery: IRequest<List<MinistryArrearUpToDateMergeDataDTO>>
    {
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string? LocationCode { get; set; }

        public class Handler : IRequestHandler<GetOnlineMinistryArrearUpToDateQuery, List<MinistryArrearUpToDateMergeDataDTO>>
        {
            private readonly IMinistryRepository _ministryRepo;
            private IMapper _mapper;
            public Handler(IMinistryRepository ministryRepo, IMapper mapper)
            {
                _ministryRepo = ministryRepo;
                _mapper = mapper;
            }

            public async Task<List<MinistryArrearUpToDateMergeDataDTO>> Handle(GetOnlineMinistryArrearUpToDateQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepo.OnlineMinistyArrearUpToDateInfo(request.BillMonth, request.ZoneCode, request.LocationCode);
                return result;
            }
        }
    }
}
