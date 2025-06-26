using Core.Application.Interfaces.MISReport;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport.RegularCustomerArrear
{
  
    public class RegularCustomerSummeryQuery : IRequest<Response<List<CenterWiseArrearSummaryDTO>>>
    {
        public string CenterCode { get; set; }
        public string BillCycleCode { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsVat { get; set; }

        public bool IsLPS { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public class Handler : IRequestHandler<RegularCustomerSummeryQuery, Response<List<CenterWiseArrearSummaryDTO>>>
        {
            private readonly IMisReportRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IMisReportRepository misReportRepository, IMapper mapper)
            {
                _repository = misReportRepository;
                _mapper = mapper;
            }

            public async Task<Response<List<CenterWiseArrearSummaryDTO>>> Handle(RegularCustomerSummeryQuery request, CancellationToken cancellationToken)
            {
                if (request.CenterCode == "0")
                {
                    var result = await _repository.GetCenterWiseArrearSummary(request.BillCycleCode, request.IsAll, request.IsPrincipal, request.IsVat, request.IsLPS, request.UserId, request.RoleId);
                    return Response<List<CenterWiseArrearSummaryDTO>>.Success(_mapper.Map<List<CenterWiseArrearSummaryDTO>>(result), "Success");
                }
                else 
                {
                    var result = await _repository.GetArrearLocationSummaryInfoAsync(request.CenterCode, request.BillCycleCode, request.IsAll, request.IsPrincipal, request.IsVat, request.IsLPS, request.UserId, request.RoleId);
                    return Response<List<CenterWiseArrearSummaryDTO>>.Success(_mapper.Map<List<CenterWiseArrearSummaryDTO>>(result), "Success");
                }
            }

           
        }

    }
}
