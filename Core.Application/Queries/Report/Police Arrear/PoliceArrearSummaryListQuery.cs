using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Police_Arrear
{
    public class PoliceArrearSummaryListQuery: IRequest<List<PoliceArrearSummaryDTO>>
    {
        public string ReportDate { get; set; }
        public string ZoneCode { get; set; }
        public string? LocationCode { get; set; }
        public string ReportType { get; set; }
        public class Handler : IRequestHandler<PoliceArrearSummaryListQuery, List<PoliceArrearSummaryDTO>> 
        {
            private readonly IMinistryRepository _ministryRepository;
            public Handler(IMinistryRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }

            public async Task<List<PoliceArrearSummaryDTO>> Handle(PoliceArrearSummaryListQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetPoliceArrearReportSummary(request.ZoneCode, request.ReportDate, request.LocationCode, request.ReportType);
                return result;
            }
        }
    }
}
