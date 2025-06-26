using Core.Application.Interfaces.Ministry;
using Core.Application.Interfaces.Police;
using Core.Domain.Police;
using MediatR;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Police
{
    public class GetOnlinePoliceQuery : IRequest<List<OnlinePoliceDetailsDTO>>
    {
        public string BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string ReportType { get; set; }



        public class Handler : IRequestHandler<GetOnlinePoliceQuery, List<OnlinePoliceDetailsDTO>>
        {

            private readonly IPoliceRepository _policeRepository;
            private readonly IMapper _mapper;

            public Handler(IPoliceRepository policeRepository, IMapper mapper)
            {
                _policeRepository = policeRepository;
                _mapper = mapper;
            }

            public async Task<List<OnlinePoliceDetailsDTO>> Handle(GetOnlinePoliceQuery request, CancellationToken cancellationToken)
            {
                List<OnlinePoliceDetailsDTO>? result = await _policeRepository.OnlinePoliceDetails(request.BillMonth, request.ZoneCode, request.LocationCode, request.ReportType);
                if (result.Count() > 0 && request.ReportType == "1")
                {
                    var OnlinePoliceData = result.GroupBy(p => p.ZoneCode).Select(
                        g => new OnlinePoliceDetailsDTO
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

                    return OnlinePoliceData.ToList();
                }
                else if (result.Count() > 0 && request.ReportType == "2")
                {
                    return _mapper.Map<List<OnlinePoliceDetailsDTO>>(result);
                }
                else
                {
                    return _mapper.Map<List<OnlinePoliceDetailsDTO>>(result);
                }
            }
        }
    }
}
