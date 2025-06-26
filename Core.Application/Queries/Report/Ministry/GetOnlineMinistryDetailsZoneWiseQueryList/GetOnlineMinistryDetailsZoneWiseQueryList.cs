using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;

namespace Core.Application.Queries.Report.Ministry.GetOnlineMinistryDetailsZoneWiseQueryList
{
    public class GetOnlineMinistryDetailsZoneWiseQueryList : IRequest<Response<List<GetCustomerArrearModelDto>>>
    {
        public string MinistryCode { get; set; }
        public string? BillMonth { get; set; }
        public string ZoneCode { get; set; }
        public string LocationCode { get; set; }
        public int NoOfMonth { get; set; }
        public int ArrearTo { get; set; }
        public int ArrearFrom { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsLps { get; set; }
        public bool IsVat { get; set; }
        public bool IsRebate { get; set; }

        public class Handler : IRequestHandler<GetOnlineMinistryDetailsZoneWiseQueryList, Response<List<GetCustomerArrearModelDto>>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<Response<List<GetCustomerArrearModelDto>>> Handle(GetOnlineMinistryDetailsZoneWiseQueryList request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetOnlineMinistryZoneWiseDetails(request.BillMonth, request.MinistryCode, request.NoOfMonth, request.ArrearFrom, request.ArrearTo, request.IsAll, request.IsPrincipal, request.IsLps, request.IsVat, request.ZoneCode, request.LocationCode, request.IsRebate);
                return Response<List<GetCustomerArrearModelDto>>.Success(_mapper.Map<List<GetCustomerArrearModelDto>>(result), "Success");
            }
        }
    }
}
