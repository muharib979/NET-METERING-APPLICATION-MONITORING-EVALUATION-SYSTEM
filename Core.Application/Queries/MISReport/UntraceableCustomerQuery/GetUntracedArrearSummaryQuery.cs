using Core.Application.Interfaces.MISReport;
using Core.Domain.Untracable;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace Core.Application.Queries.MISReport.UntraceableCustomerQuery
{
    public class GetUntracedArrearSummaryQuery : IRequest<UntracedPenaltyAndSupplementryList>
    {
        public string ZoneCode { get; set; }
        public string BillMonth { get; set; }


        public class Handler : IRequestHandler<GetUntracedArrearSummaryQuery, UntracedPenaltyAndSupplementryList>
        {
            private readonly IUntraceableCustomerRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUntraceableCustomerRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<UntracedPenaltyAndSupplementryList> Handle(GetUntracedArrearSummaryQuery request, CancellationToken cancellationToken)
            {
                var margeData = new List<UntracedCustArrearMergeSummaryDto>();
                var result = await _repository.GetUntracedArrearSummary(request.ZoneCode, request.BillMonth);
                var previousResult = result.Item2;

                #region Untraceable Summary methods
                var ucSummaryPreviousDataZoneWise = previousResult.GroupBy(c => c.ZONE_CODE).Select(
                    g => new UntracedCustArrearMergeSummaryDto
                    {
                        ZoneCode = g.First().ZONE_CODE,
                        ZoneName = g.First().ZONE_NAMEBN,
                        PreMonTracedCustCount = g.Count(),
                        PreMonTracedCustArrear = g.Sum(d => d.ARREAR_PRICIPAL) + g.Sum(d => d.ARREAR_LPS) + g.Sum(d => d.ARREAR_VAT),
                        PreMonTracedCustReceipt = g.Sum(d => d.ARREAR_RECEIPT_PRINCIPAL) + +g.Sum(d => d.ARREAR_RECEIPT_VAT),
                    });

                var currResult = result.Item1;
                var ucSummaryCurrDataZoneWise = currResult.GroupBy(c => c.ZONE_CODE).Select(
                   g => new UntracedCustArrearMergeSummaryDto
                   {
                       ZoneCode = g.First().ZONE_CODE,
                       ZoneName = g.First().ZONE_NAMEBN,
                       CurrMonTracedCustCount = g.Count(),
                       CurrMonTracedCustArrear = g.Sum(d => d.CURR_PRINCIPAL) + g.Sum(d => d.CURR_VAT) + g.Sum(d => d.CURR_LPS),
                       CurrMonTracedCustReceipt = g.Sum(d => d.CURR_RECEIPT_PRINCIPAL) + +g.Sum(d => d.CURR_RECEIPT_VAT),
                   });


                var combinedList = ucSummaryCurrDataZoneWise.Concat(ucSummaryPreviousDataZoneWise);

                var mergeData = combinedList.GroupBy(c => c.ZoneCode).Select(
                 g => new UntracedCustArrearMergeSummaryDto
                 {
                     ZoneCode = g.First().ZoneCode,
                     ZoneName = g.First().ZoneName,
                     CurrMonTracedCustCount = g.Sum(c => c.CurrMonTracedCustCount),
                     CurrMonTracedCustArrear = g.Sum(c => c.CurrMonTracedCustArrear),
                     CurrMonTracedCustReceipt = g.Sum(d => d.CurrMonTracedCustReceipt),

                     PreMonTracedCustCount = g.Sum(d => d.PreMonTracedCustCount),
                     PreMonTracedCustArrear = g.Sum(d => d.PreMonTracedCustArrear),
                     PreMonTracedCustReceipt = g.Sum(d => d.PreMonTracedCustReceipt),

                     TotalCurrMonTracedCustCount = g.Sum(c => c.CurrMonTracedCustCount) + g.Sum(d => d.PreMonTracedCustCount),
                     TotalCurrMonTracedCustArrear = g.Sum(c => c.CurrMonTracedCustArrear) + g.Sum(d => d.PreMonTracedCustArrear),
                     TotalCurrMonTracedCustReceipt = g.Sum(d => d.CurrMonTracedCustReceipt) + g.Sum(d => d.PreMonTracedCustReceipt)
                 });
                #endregion



                return new UntracedPenaltyAndSupplementryList { UntraceableDataList= mergeData.ToList(), PenaltyDataList =result.Item3, SupplementaryDataList = result.Item4 };
            }
        }
    }

    public class UntracedPenaltyAndSupplementryList
    {
        public List<UntracedCustArrearMergeSummaryDto> UntraceableDataList { get; set; }
        public List<UntracedCustArrearMergeSummaryDto> PenaltyDataList { get; set; }
        public List<UntracedCustArrearMergeSummaryDto> SupplementaryDataList { get; set; }
    }
}
