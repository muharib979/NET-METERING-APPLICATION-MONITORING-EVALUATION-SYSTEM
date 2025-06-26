using Core.Application.Interfaces.MISReport;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport.AllCustomerArrear
{
    public class AllCustomerArrearDetailsQuery: IRequest<Response<List<CustomerArrearDto>>>
    {
        public string[] DbCodes { get; set; }
        public string[]? LocationCodes { get; set; }
        public string BillMonth { get; set; }
        public int FromAmount { get; set; }
        public int ToAmount { get; set; }
        public string Tariff { get; set; }
        public string BillGroupId { get; set; }
        public string BookId { get; set; }
        public int NoOfMonth { get; set; }
        public int ReportTypeId { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsVAT { get; set; }
        public bool IsLPS { get; set; }
        public string OrderTypeId { get; set; }
        public int UserId { get; set; } 
        public int RoleId { get; set; }

        public class Handler : IRequestHandler<AllCustomerArrearDetailsQuery, Response<List<CustomerArrearDto>>>
        {
            private readonly IMisReportRepository _misReportRepository;
            private readonly IMapper _mapper;
            public Handler(IMisReportRepository misReportRepository, IMapper mapper)
            {
                _misReportRepository= misReportRepository;
                _mapper= mapper;
            }
            public async Task<Response<List<CustomerArrearDto>>> Handle(AllCustomerArrearDetailsQuery request, CancellationToken cancellationToken)
            {
                var result = await _misReportRepository.AllCustomerArrearDetails(request.DbCodes,request.LocationCodes,request.FromAmount,request.ToAmount, request.Tariff, request.BillGroupId, request.BookId,request.NoOfMonth,request.BillMonth, request.ReportTypeId, request.IsAll, request.IsPrincipal, request.IsVAT, request.IsLPS, request.OrderTypeId, request.UserId, request.RoleId);
                return Response<List<CustomerArrearDto>>.Success(_mapper.Map<List<CustomerArrearDto>>(result), "Success");
            }
        }
    }
}
