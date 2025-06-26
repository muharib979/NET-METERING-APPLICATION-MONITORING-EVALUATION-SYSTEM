using Core.Application.Interfaces.Ministry;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllMinistryDetailsSummaryQuery
{
    public class GetAllMinistryDetailsSummaryQuery: IRequest<List<MinistryDetailsSummaryMergeDTO>>
    {
        public string MinistryCode { get; set; }
        public string? BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public int NoOfMonth { get; set; }
        public int ArrearTo { get; set; }
        public int ArrearFrom { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsLps { get; set; }
        public bool IsVat { get; set; }
        public bool IsRebate { get; set; }

        public class Handler : IRequestHandler<GetAllMinistryDetailsSummaryQuery, List<MinistryDetailsSummaryMergeDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<List<MinistryDetailsSummaryMergeDTO>> Handle(GetAllMinistryDetailsSummaryQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetOnlineMinistryZoneWiseSummary(request.BillMonth, request.MinistryCode, request.NoOfMonth, request.ArrearFrom, request.ArrearTo, request.IsAll, request.IsPrincipal, request.IsLps, request.IsVat, request.ZoneCode, request.LocationCode, request.IsRebate);
                if (result.Count() > 0) 
                {
                    var ministryDetailsSummaryData = result.GroupBy(p => p.ZoneCode).Select(
                        g => new MinistryDetailsSummaryMergeDTO 
                        { 
                            ZoneCode = g.First().ZoneCode,
                            ZoneName = g.First().ZoneName,
                            ZoneNameBn = g.First().ZoneNameBn,
                            MinistryNameBn = g.First().MinistryNameBn,
                            MinistryCode = g.First().MinistryCode,
                            Prn = g.Sum(c => c.Prn),
                            Lps = g.Sum(c => c.Lps),
                            Vat = g.Sum(c => c.Vat),
                            Total = g.Sum(c => c.Total),
                            ConsumerNo = g.Count()
                        }).OrderBy(c => c.ZoneCode);
                    return ministryDetailsSummaryData.ToList();
                }
                else
                {
                    return _mapper.Map<List<MinistryDetailsSummaryMergeDTO>>(result);
                }
            }
        }
    }
}
