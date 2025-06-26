using Core.Application.Interfaces.MISReport;
using Shared.DTOs.MISReport;

namespace Core.Application.Queries.MISReport.AllCustomerArrear
{
    public class GetPrepaidCustomerArrearBasedOnOffsetQuery : IRequest<List<PrepaidCustomerArrearBasedOnOffsetDTO>>
    {

        public string? ConsumerNo { get; set; }
        public string? Tariff { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public int Status { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsLps { get; set; }
        public bool IsVat { get; set; }
        public string OrderBy { get; set; }
        public string ReportType { get; set; }
        public int UserId { get; set; } = 0;
        public int RoleId { get; set; } = 0;
        public string[] DbCodes { get; set; }
        public string[]? LocationCodes { get; set; }

        public class Handler : IRequestHandler<GetPrepaidCustomerArrearBasedOnOffsetQuery, List<PrepaidCustomerArrearBasedOnOffsetDTO>>
        {
            private readonly IMisReportRepository _misReportRepository;
            private readonly IMapper _mapper;
            public Handler(IMisReportRepository misReportRepository, IMapper mapper)
            {
                _misReportRepository = misReportRepository;
                _mapper = mapper;
            }
            public async Task<List<PrepaidCustomerArrearBasedOnOffsetDTO>> Handle(GetPrepaidCustomerArrearBasedOnOffsetQuery request, CancellationToken cancellationToken)
            {
                if (request.ReportType == "1") // summary
                {
                    var result = await _misReportRepository.PrepaidCustomerArrarBasedOnOffsetSummary(request.DbCodes, request.LocationCodes, request.ConsumerNo, request.Tariff, request.FromAmount, request.ToAmount, request.Status, request.IsAll, request.IsPrincipal, request.IsVat, request.IsLps, request.OrderBy, request.ReportType, request.UserId, request.RoleId);
                    return result.ToList();
                }
                else
                {
                    var result = await _misReportRepository.PrepaidCustomerArrarBasedOnOffset(request.DbCodes, request.LocationCodes, request.ConsumerNo, request.Tariff, request.FromAmount, request.ToAmount, request.Status, request.IsAll, request.IsPrincipal, request.IsVat, request.IsLps, request.OrderBy, request.ReportType, request.UserId, request.RoleId);
                    return result.ToList();
                }

            }
        }
    }
}
