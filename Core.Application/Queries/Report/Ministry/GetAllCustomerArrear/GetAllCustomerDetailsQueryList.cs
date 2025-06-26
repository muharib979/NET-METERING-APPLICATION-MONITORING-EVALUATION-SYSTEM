using Core.Application.Interfaces.Ministry;
using Core.Domain.Ministry;

namespace Core.Application.Queries.Report.Ministry.GetAllCustomerArrear
{
    public class GetAllCustomerDetailsQueryList : IRequest<AllCustomerMode>
    {
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
        public string ReportType { get; set; }

        public class Handler : IRequestHandler<GetAllCustomerDetailsQueryList, AllCustomerMode>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<AllCustomerMode> Handle(GetAllCustomerDetailsQueryList request, CancellationToken cancellationToken)
            {
                //var result = await _ministryRepository.GetAllCustomerDetails(request.BillMonth, request.NoOfMonth, request.ArrearFrom, request.ArrearTo, request.IsAll, request.IsPrincipal, request.IsLps, request.IsVat, request.DbCode, request.LocationCode);
                //return result;

                var result = await _ministryRepository.GetAllCustomerDetails(request.BillMonth, request.NoOfMonth, request.ArrearFrom, request.ArrearTo, request.IsAll, request.IsPrincipal, request.IsLps, request.IsVat, request.DbCode, request.LocationCode);

                if (result.PrivateCustomer.Count()  > 0 && request.ReportType == "1")
                {
                    var ministrySummary = result.MinistryCustomer.GroupBy(p => p.IS_GOVT_CUSTOMER=="1").Select(
                        g => new GetCustomerArrearModel
                        {
                            LOCATION_CODE = g.First().LOCATION_CODE,
                            LOCATION_DESC = g.First().LOCATION_DESC,
                            PRN = g.Sum(c => c.PRN),
                            LPS = g.Sum(c => c.LPS),
                            VAT = g.Sum(c => c.VAT),
                            TOTAL = g.Sum(c => c.TOTAL),
                        });

                    var privateSummary = result.PrivateCustomer.GroupBy(p => p.IS_GOVT_CUSTOMER == "0").Select(
                      g => new GetCustomerArrearModel
                      {
                          LOCATION_CODE = g.First().LOCATION_CODE,
                          LOCATION_DESC = g.First().LOCATION_DESC,
                          PRN = g.Sum(c => c.PRN),
                          LPS = g.Sum(c => c.LPS),
                          VAT = g.Sum(c => c.VAT),
                          TOTAL = g.Sum(c => c.TOTAL),
                      });
                    return new AllCustomerMode { MinistryCustomer = ministrySummary.ToList(), PrivateCustomer = privateSummary.ToList() };
                }
                else
                {

                    return result;
                }
            }
        }
    }
}

