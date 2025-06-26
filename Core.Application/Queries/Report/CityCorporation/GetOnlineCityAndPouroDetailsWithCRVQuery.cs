using Core.Application.Interfaces.CityCorporation;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.CityCorporation
{
    public class GetOnlineCityAndPouroDetailsWithCRVQuery: IRequest<OnlineCityPourDataListModel>
    {
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetOnlineCityAndPouroDetailsWithCRVQuery, OnlineCityPourDataListModel>
        {
            private readonly ICityCorporationRepository _cityRepository;
            private readonly IMapper _mapper;
            public Handler(ICityCorporationRepository cityRepository, IMapper mapper)
            {
                _cityRepository = cityRepository;
                _mapper = mapper;
            }

            public async Task<OnlineCityPourDataListModel> Handle(GetOnlineCityAndPouroDetailsWithCRVQuery request, CancellationToken cancellationToken)
            {
                var result = _mapper.Map<List<OnlineCityPourMergeDTO>>(await _cityRepository.GetOnlinePouroAndCityDetailsWithCRV(request.BillMonth, request.ZoneCode, request.LocationCode, request.ReportType));
                if (result.Count > 0 && request.ReportType == "1")
                {
                    var cityCorporationData = result.GroupBy(c => c.CityCorporationCode).Select(
                    g => new OnlineCityPourMergeDTO
                    {
                        CityCorporationCode = g.First().CityCorporationCode,
                        CityCorporationName = g.First().CityCorporationName,
                        // PouroshovaCode = g.First().PouroshovaCode,
                        TotalReceiptArrear = g.Sum(c => c.TotalReceiptArrear),
                        TotalArrearAmount = g.Sum(c => c.TotalArrearAmount),
                        ArrearReceiptAmount = g.Sum(c => c.ArrearReceiptAmount),
                        CurrReceiptVat = g.Sum(c => c.CurrReceiptVat),
                        CurrReceiptPrincipal = g.Sum(c => c.CurrReceiptPrincipal),
                        CurrReceiptAmt = g.Sum(c => c.CurrReceiptAmt),
                        CurrLps = g.Sum(c => c.CurrLps),
                        CurrPrin = g.Sum(c => c.CurrPrin),
                        CurrVat = g.Sum(c => c.CurrVat),

                        ArrearLps = g.Sum(c => c.ArrearLps),
                        ArrearPrincipal = g.Sum(c => c.ArrearPrincipal),
                        ArrearVat = g.Sum(c => c.ArrearVat),

                        TotalReceiptPrincipal = g.Sum(c => c.TotalReceiptPrincipal),

                        TotalReceiptVat = g.Sum(c => c.TotalReceiptVat),


                        ZoneName = g.First().ZoneName,
                        ZoneCode = g.First().ZoneCode,
                        CityCorporationNameBn = g.First().CityCorporationNameBn,
                        ReceiptBillMonth = g.First().ReceiptBillMonth,
                        PrvReceiptPrincipal = g.Sum(c => c.PrvReceiptPrincipal),
                        PrvReceiptVat = g.Sum(c => c.PrvReceiptVat),
                        PrvReceiptAmt = g.Sum(c => c.PrvReceiptAmt),
                        OrderNo = g.First().OrderNo
                    }).OrderBy(c => c.OrderNo);

                    var pouroshovaData = result.GroupBy(c => c.PouroshovaCode).Select(
                    g => new OnlineCityPourMergeDTO
                    {
                        PouroshovaCode = g.First().PouroshovaCode,
                        PouroName = g.First().PouroName,
                        // PouroshovaCode = g.First().PouroshovaCode,
                        TotalReceiptArrear = g.Sum(c => c.TotalReceiptArrear),
                        TotalArrearAmount = g.Sum(c => c.TotalArrearAmount),
                        ArrearReceiptAmount = g.Sum(c => c.ArrearReceiptAmount),
                        CurrReceiptVat = g.Sum(c => c.CurrReceiptVat),
                        CurrReceiptPrincipal = g.Sum(c => c.CurrReceiptPrincipal),
                        CurrReceiptAmt = g.Sum(c => c.CurrReceiptAmt),
                        CurrLps = g.Sum(c => c.CurrLps),
                        CurrPrin = g.Sum(c => c.CurrPrin),
                        CurrVat = g.Sum(c => c.CurrVat),

                        ArrearLps = g.Sum(c => c.ArrearLps),
                        ArrearPrincipal = g.Sum(c => c.ArrearPrincipal),
                        ArrearVat = g.Sum(c => c.ArrearVat),

                        TotalReceiptPrincipal = g.Sum(c => c.TotalReceiptPrincipal),

                        TotalReceiptVat = g.Sum(c => c.TotalReceiptVat),
                        ZoneName = g.First().ZoneName,
                        ZoneCode = g.First().ZoneCode,
                        PouroNameBn = g.First().PouroNameBn,
                        ReceiptBillMonth = g.First().ReceiptBillMonth,
                        PrvReceiptPrincipal = g.Sum(c => c.PrvReceiptPrincipal),
                        PrvReceiptVat = g.Sum(c => c.PrvReceiptVat),
                        PrvReceiptAmt = g.Sum(c => c.PrvReceiptAmt),
                        OrderNo = g.First().OrderNo,
                    }).OrderBy(c => c.OrderNo).ThenBy(C => C.PouroshovaCode);

                    return new OnlineCityPourDataListModel { CityCoporationDataList = cityCorporationData.ToList(), PouroshovaDataList = pouroshovaData.ToList() };
                }
                else
                {
                    var detailsResult = _mapper.Map<List<OnlineCityPourMergeDTO>>(result);

                    return new OnlineCityPourDataListModel { CityCoporationDataList = null, PouroshovaDataList = null, CityPouroshovaDetailsDataList = detailsResult };
                }
            }
        }
    }
}
