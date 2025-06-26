using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Police_Arrear
{
    public class GetAllPoliceArrearDetailsListQuery: IRequest<List<PoliceArrearDetailsDTO>>
    {
        public string ReportDate { get; set; }
        public string ZoneCode { get; set; }
        public string? LocationCode { get; set; }
        public string ReportType { get; set; }
        public class Handler : IRequestHandler<GetAllPoliceArrearDetailsListQuery, List<PoliceArrearDetailsDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            public Handler(IMinistryRepository ministryRepository)
            {
                _ministryRepository = ministryRepository;
            }

            public async Task<List<PoliceArrearDetailsDTO>> Handle(GetAllPoliceArrearDetailsListQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetPoliceArrearReportDetails(request.ZoneCode,request.ReportDate,request.LocationCode,request.ReportType);
                return result;
            }
        }
    }
}
