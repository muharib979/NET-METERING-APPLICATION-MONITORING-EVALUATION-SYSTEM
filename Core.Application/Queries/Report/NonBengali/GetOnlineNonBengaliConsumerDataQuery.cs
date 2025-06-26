using Core.Application.Interfaces.NonBengali;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.NonBengali
{
    public class GetOnlineNonBengaliConsumerDataQuery : IRequest<OnlineNonBengaliModel>
    {
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string BillMonth { get; set; }
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetOnlineNonBengaliConsumerDataQuery, OnlineNonBengaliModel>
        {
            private readonly INonBengaliRepository _nonBengaliRepository;
            private readonly IMapper _mapper;
            public Handler(INonBengaliRepository nonBengaliRepository, IMapper mapper)
            {
                _nonBengaliRepository = nonBengaliRepository;
                _mapper = mapper;
            }
            public async Task<OnlineNonBengaliModel> Handle(GetOnlineNonBengaliConsumerDataQuery request, CancellationToken cancellationToken)
            {
                var result = await _nonBengaliRepository.GetOnlineNonBengaliConsumerDataByDate(request.ZoneCode, request.LocationCode, request.BillMonth, request.ReportType);
                var staticResult = await _nonBengaliRepository.GetNonBengaliStaticValue(request.ZoneCode, request.LocationCode);
                if (result.Count() > 0 && request.ReportType == "1")
                {
                    var nonBengaliSummaryData = result.GroupBy(c => c.NonBengaliCampCode).Select(
                        g => new OnlineNonBengaliSummaryDTO
                        {
                            
                            NonBengaliCampCode = g.First().NonBengaliCampCode,
                            NonbengaliName = g.First().NonbengaliName,
                            NonBengaliNameBn = g.First().NonBengaliNameBn,
                            LocationCode = g.First().LocationCode,
                            LocationDesc = g.First().LocationDesc,
                            LocationNameBn = g.First().LocationNameBn,
                            BillCycleCode = g.First().BillCycleCode,
                            MeterCount = g.Count(),
                            Prn = g.Sum(c => c.CurrPrin + c.CurrVat),
                            Lps = g.Sum(c => c.CurrLps),
                            Vat = g.First().CurrVat,
                            //Vat= g.Sum(c => c.CurrVat + c.ArrearVat),
                            //TotalAmount = g.Sum(c => c.CurrPrin + c.ArrearPrincipal + c.CurrLps + c.ArrearLps)
                            TotalAmount = g.Sum(c => c.CurrPrin + c.CurrLps + c.CurrVat)
                        }).OrderBy(c=> c.NonBengaliCampCode);
                    return new OnlineNonBengaliModel { NonbengliDetailsDynamicDataList = nonBengaliSummaryData.ToList(), NonbengliDetailsStaticDataList = staticResult, NonbengliDetailsDataList = null };
                }
                else
                {
                    return new OnlineNonBengaliModel { NonbengliDetailsDynamicDataList = null, NonbengliDetailsStaticDataList = null, NonbengliDetailsDataList = _mapper.Map<List<OnlineNonBengaliSummaryDTO>>(result) };
                }

            }
        }
    }
}
