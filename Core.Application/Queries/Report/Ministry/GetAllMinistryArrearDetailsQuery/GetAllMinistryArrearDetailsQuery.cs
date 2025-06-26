using Core.Application.Interfaces.Ministry;
using Shared.DTOs.CityCorporation;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllMinistryArrearDetailsQuery
{
    public class GetAllMinistryArrearDetailsQuery: IRequest<List<OnlineMinistryArrearDetailsMergeDTO>>
    {
        public string BillMonth { get; set; }
        public string MinistryCode { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetAllMinistryArrearDetailsQuery, List<OnlineMinistryArrearDetailsMergeDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<List<OnlineMinistryArrearDetailsMergeDTO>> Handle(GetAllMinistryArrearDetailsQuery request, CancellationToken cancellationToken)
            {
                List<OnlineMinistryArrearDetailsMergeDTO> result = new List<OnlineMinistryArrearDetailsMergeDTO>();
                if (request.ReportType != "4" && request.ReportType != "5") { 
                    result = await _ministryRepository.OnlineMinistyArrearDetails(request.BillMonth, request.MinistryCode, request.ZoneCode, request.LocationCode, request.ReportType);
            }
                else
                {
                    // result = await _ministryRepository.OnlineCRVPaymentDetails(request.BillMonth, request.MinistryCode, request.ZoneCode, request.LocationCode, request.ReportType);
                    result = await _ministryRepository.OnlineMinistyArrearDetailsWithCRV(request.BillMonth, request.MinistryCode, request.ZoneCode, request.LocationCode, request.ReportType);

                }
                if (result.Count() > 0 && request.ReportType == "1")
                {
                    var ministryArrearData = result.GroupBy(p => p.ZoneCode).Select(
                        g => new OnlineMinistryArrearDetailsMergeDTO
                        {
                            ConsumerNo = g.Count(),
                            CustomerName = g.First().CustomerName,
                            MinistryCode = g.First().MinistryCode,
                            MinistryName = g.First().MinistryName,
                            MinistryNameBn = g.First().MinistryNameBn,
                            ZoneCode = g.First().ZoneCode,
                            ZoneName = g.First().ZoneName,
                            ZoneNameBn = g.First().ZoneNameBn,
                            CurrReceiptVat = g.Sum(c => c.CurrReceiptVat),
                            CurrReceiptPrincipal = g.Sum(c => c.CurrReceiptPrincipal),
                            CurrLps = g.Sum(c => c.CurrLps),
                            CurrPrincipal = g.Sum(c => c.CurrPrincipal),
                            CurrVat = g.Sum(c => c.CurrVat),
                            ArrearLps = g.Sum(c => c.ArrearLps),
                            ArrearPrincipal = g.Sum(c => c.ArrearPrincipal),
                            ArrearVat = g.Sum(c => c.ArrearVat),
                            TotalReceiptArrear = g.Sum(c => c.TotalReceiptArrear),
                            ArrearReceiptPrincipal = g.Sum(c => c.ArrearReceiptPrincipal),
                            ArrearReceiptVat = g.Sum(c => c.ArrearReceiptVat),
                        });

                    return ministryArrearData.ToList();
                }
                else if (result.Count() > 0 && request.ReportType == "2")
                {
                    return _mapper.Map<List<OnlineMinistryArrearDetailsMergeDTO>>(result);
                }
                else 
                {
                    return _mapper.Map<List<OnlineMinistryArrearDetailsMergeDTO>>(result);
                }
                //return _mapper.Map<List<OnlineMinistryArrearDetailsMergeDTO>>(result);
            }
        }
    }
}
