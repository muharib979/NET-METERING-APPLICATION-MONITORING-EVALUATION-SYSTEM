using Core.Application.Interfaces;
using Core.Application.Interfaces.Ministry;
using Core.Domain.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetPublicSecurityDivisionQuery
{
    public class GetSecurityServiceDivisionQuery : IRequest<List<PublicSecurityDivisionMergeDTO>>
    {
        public string BillMonth { get; set; }
      //  public string MinistryCode { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string ReportType { get; set; }
        public class Handler : IRequestHandler<GetSecurityServiceDivisionQuery, List<PublicSecurityDivisionMergeDTO>>
        {
            private readonly IFireServiceRepository _fireServiceRepository;
            private readonly IMapper _mapper;
            public Handler(IFireServiceRepository fireServiceRepository, IMapper mapper)
            {
                _fireServiceRepository = fireServiceRepository;
                _mapper = mapper;
            }
            public async Task<List<PublicSecurityDivisionMergeDTO>> Handle(GetSecurityServiceDivisionQuery request, CancellationToken cancellationToken)
            {
                var result = await _fireServiceRepository.OnlineSecurityServiceDivArrearDetails(request.BillMonth,  request.ZoneCode, request.LocationCode, request.ReportType);
                if (result.Count() > 0 && request.ReportType == "1")
                {
                    var ministryArrearData = result.GroupBy(p => p.ZoneCode).Select(
                        g => new PublicSecurityDivisionMergeDTO
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
                //else if (result.Count() > 0 && request.ReportType == "2")
                //{
                //    return _mapper.Map<List<PublicSecurityDivisionMergeDTO>>(result);
                //}
                else
                {
                    return _mapper.Map<List<PublicSecurityDivisionMergeDTO>>(result);
                }
            }
        }
    }

}
   
