using Core.Application.Interfaces.Agriculture.RepositoryInterfaces;
using Shared.DTOs.Agriculture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.agriculture.GetFinancialYearWiseOnlineKrishiPoultryLedger
{
    public class GetOnlineKrishiPoultryLedgerQuery : IRequest<List<OnlineAgricultureLedgerMergeDTO>>
    {
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }

        public class Handler : IRequestHandler<GetOnlineKrishiPoultryLedgerQuery, List<OnlineAgricultureLedgerMergeDTO>>
        {
            private readonly IAgricultureRepository _agricultureRepository;
            private readonly IMapper _mapper;
            public Handler(IAgricultureRepository agricultureRepository, IMapper mapper)
            {
                _agricultureRepository = agricultureRepository;
                _mapper = mapper;
            }

            public async Task<List<OnlineAgricultureLedgerMergeDTO>> Handle(GetOnlineKrishiPoultryLedgerQuery request, CancellationToken cancellationToken)
            {
                var result = await _agricultureRepository.GetOnlineAgriAndPoultryLedger(request.StartMonth, request.EndMonth, request.ZoneCode, request.LocationCode);
                return _mapper.Map<List<OnlineAgricultureLedgerMergeDTO>>(result);
            }
        }
    }
}
