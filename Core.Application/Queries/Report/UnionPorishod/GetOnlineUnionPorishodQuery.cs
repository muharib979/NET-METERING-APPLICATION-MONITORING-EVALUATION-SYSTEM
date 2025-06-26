using Core.Application.Interfaces.UnionPorishod;
using DocumentFormat.OpenXml.Spreadsheet;
using Shared.DTOs.CityCorporation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.UnionPorishod
{
    public class GetOnlineUnionPorishodQuery : IRequest<List<ZoneWiseUnionPorishodDto>>
    {

        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public string? BillMonth { get; set; }
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetOnlineUnionPorishodQuery, List<ZoneWiseUnionPorishodDto>>
        {
            private readonly IUnionPorishodRepository _repository;
            private IMapper _mapper;

            public Handler(IUnionPorishodRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<ZoneWiseUnionPorishodDto>> Handle(GetOnlineUnionPorishodQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetOnlineUnionPorishod(request.ZoneCode, request.LocationCode, request.BillMonth, request.ReportType);

                if (result.Count() > 0 && request.ReportType == "1")
                {
                    var unionPorishadSummaryData = result.GroupBy(c => c.UnionPorishadCode).Select(
                    g => new ZoneWiseUnionPorishodDto
                    {
                        //zoneName, unionPorishodName, prevMonthArrearAmt,currMonthArrearAmount,totalReceiptAmt,totalArrearAmt

                        ZoneName = g.First().ZoneName,
                        ZoneCode = g.First().ZoneCode,
                        UnionPorishodName = g.First().UnionPorishadNameBn,
                        CurrMonthArrearAmount = g.Sum(c => c.CurrPrin + c.CurrLps + c.CurrVat),
                        PrevMonthArrearAmt = g.Sum(c => c.ArrearPrincipal + c.ArrearLps + c.ArrearVat),
                        ReceiptBillMonth = g.First().ReceiptBillMonth,
                        //TotalReceiptAmt = g.Sum(c => c.TotalReceiptPrincipal + c.TotalReceiptVat),
                        TotalReceiptAmt = g.Sum(c => c.CurrReceiptPrincipal + c.CurrReceiptVat),
                        TotalReceiptArrear = g.Sum(c => c.TotalReceiptArrear)

                    });

                    return unionPorishadSummaryData.ToList();
                }
                else
                {
                    return _mapper.Map<List<ZoneWiseUnionPorishodDto>>(result);
                }
            }
        }
    }
}
