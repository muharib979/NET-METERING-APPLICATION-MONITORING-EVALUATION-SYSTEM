using Core.Application.Interfaces.Ministry;
using Core.Domain.Ministry;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;

namespace Core.Application.Queries.Report.Ministry.GetOldMinistryDetailsQueryList
{
    public class GetOldMinistryDetailsQueryList : IRequest<Response<List<GetCustomerArrearModelDto>>>
    {
        public string MinistryCode { get; set; }
        public string? BillMonth { get; set; }
        public string DbCode { get; set; }
        public string LocationCode { get; set; }
        public int NoOfMonth { get; set; }
        public int ArrearTo { get; set; }
        public int ArrearFrom { get; set; }
        public bool IsAll { get; set; }
        public bool IsPrincipal { get; set; }
        public bool IsLps { get; set; }
        public bool IsVat { get; set; }

        public class Handler : IRequestHandler<GetOldMinistryDetailsQueryList, Response<List<GetCustomerArrearModelDto>>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<Response<List<GetCustomerArrearModelDto>>> Handle(GetOldMinistryDetailsQueryList request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetOldMinistryDetails(request.BillMonth, request.MinistryCode, request.NoOfMonth, request.ArrearFrom, request.ArrearTo, request.IsAll, request.IsPrincipal, request.IsLps, request.IsVat, request.DbCode, request.LocationCode);
                return Response<List<GetCustomerArrearModelDto>>.Success(_mapper.Map<List<GetCustomerArrearModelDto>>(result), "Success");
            }
        }
    }
}
