using Core.Application.Interfaces.MISReport;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport.AllCustomerArrear
{
    public class AllCustomerArrearSummaryQuery: IRequest<Response<List<AllCustomerArrearSummaryDto>>>
    {
        public string CenterCode { get; set; }
        public string BillMonth { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsVat { get; set; }
        public bool IsLps { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public class Handler : IRequestHandler<AllCustomerArrearSummaryQuery, Response<List<AllCustomerArrearSummaryDto>>>
        {
            private readonly IMisReportRepository _misReportRepository;
            private readonly IMapper _mapper;
            public Handler(IMisReportRepository misReportRepository, IMapper mapper)
            {
                _misReportRepository= misReportRepository;
                _mapper= mapper;
            }
            public async Task<Response<List<AllCustomerArrearSummaryDto>>> Handle(AllCustomerArrearSummaryQuery request, CancellationToken cancellationToken)
            {

                if(request.CenterCode == "0")
                {
                    var result = await _misReportRepository.AllCustomerArrearSummaryCenterWise(request.BillMonth, request.IsAll, request.IsPrincipal, request.IsVat, request.IsLps, request.UserId, request.RoleId);
                    return Response<List<AllCustomerArrearSummaryDto>>.Success(_mapper.Map<List<AllCustomerArrearSummaryDto>>(result), "Success");
                }
                else
                {
                    var result = await _misReportRepository.GetAllCustomerArrearLocationSummaryData(request.CenterCode, request.BillMonth, request.IsAll, request.IsPrincipal, request.IsVat, request.IsLps, request.UserId, request.RoleId);
                    return Response<List<AllCustomerArrearSummaryDto>>.Success(_mapper.Map<List<AllCustomerArrearSummaryDto>>(result), "Success");
                }
               
            }
        }
    }
}
