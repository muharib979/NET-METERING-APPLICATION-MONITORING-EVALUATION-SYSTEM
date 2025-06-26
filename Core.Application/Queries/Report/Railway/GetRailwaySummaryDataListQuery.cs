using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Railway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Railway
{
    public class GetRailwaySummaryDataListQuery: IRequest<List<RailwaySummaryDTO>>
    {
        public string ReportDate { get; set; }
        public string ZoneCode { get; set; }
        public string? LocationCode { get; set; }
        public string ReportType { get; set; }
        public string CircleCode { get; set; }
        public class Handler : IRequestHandler<GetRailwaySummaryDataListQuery, List<RailwaySummaryDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            public Handler(IMinistryRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }

            public async Task<List<RailwaySummaryDTO>> Handle(GetRailwaySummaryDataListQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetRailwaySummary(request.ZoneCode, request.ReportDate, request.LocationCode, request.ReportType,request.CircleCode);
                return result;
            }
        }
    }
}
