using Core.Application.Interfaces.MISReport;
using Core.Domain.Untracable;
using Shared.DTOs.MISReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport.UntraceableCustomerQuery
{
    public class GetUntracedCustArrearCollectionQuery : IRequest<List<UntracebleCustArrearReportDTO>>
    {
        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string LocationCode { get; set; }
        public string BillMonth { get; set; }
      

        public class Handler : IRequestHandler<GetUntracedCustArrearCollectionQuery, List<UntracebleCustArrearReportDTO>>
        {
            private readonly IUntraceableCustomerRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUntraceableCustomerRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<UntracebleCustArrearReportDTO>> Handle(GetUntracedCustArrearCollectionQuery request, CancellationToken cancellationToken)
            {
                //var result = await _repository.GetUntracedArrearDetails(request.ZoneCode, request.CircleCode, request.LocationCode, request.BillMonth);
				var margeData = new List<UntracedCustArrearMergeSummaryDto>();
				var result = await _repository.GetUntracedArrearDetails(request.ZoneCode, request.CircleCode, request.LocationCode, request.BillMonth);

				var allUntracedCustResult = result.Item1;
				var ucDetailsAllDataLocWise = allUntracedCustResult.GroupBy(c => c.LOCATION_CODE).Select(
				   g => new UntracebleCustArrearReportDTO
				   {
					   ZoneCode = g.First().ZONE_CODE,
					   ZoneName = g.First().ZONE_NAMEBN,
					   CircleCode= g.First().CIRCLE_CODE,
					   CircleNameBn= g.First().CIRCLE_NAMEBN,
					   LocationCode=g.First().LOCATION_CODE,
					   LOCATIONNAMEBN= g.First().LOCATION_NAMEBN,
					   TotalUntracedCustCount = g.Count(),
					   TotalUntracedCustArrear = g.Sum(d => d.CURR_PRINCIPAL) + g.Sum(d => d.CURR_VAT) + g.Sum(d => d.CURR_LPS) + g.Sum(d => d.ARREAR_PRICIPAL) + g.Sum(d => d.ARREAR_LPS) + g.Sum(d => d.ARREAR_VAT) - 
												(g.Sum(d => d.ARREAR_RECEIPT_PRINCIPAL) + g.Sum(d => d.ARREAR_RECEIPT_VAT) + g.Sum(d => d.CURR_RECEIPT_PRINCIPAL) + g.Sum(d => d.CURR_RECEIPT_VAT)),
				   });

				var currResult = result.Item2;
				var ucDetailsCurrDataLocWise = currResult.GroupBy(c => c.LOCATION_CODE).Select(
				   g => new UntracebleCustArrearReportDTO
				   {
					   ZoneCode = g.First().ZONE_CODE,
					   ZoneName = g.First().ZONE_NAMEBN,
					   CircleCode = g.First().CIRCLE_CODE,
					   CircleNameBn = g.First().CIRCLE_NAMEBN,
					   LocationCode = g.First().LOCATION_CODE,
					   LOCATIONNAMEBN = g.First().LOCATION_NAMEBN,
					   CurrMonTracedCustCount = g.Count(),
					   CurrMonTracedCustArrear = g.Sum(d => d.CURR_PRINCIPAL) + g.Sum(d => d.CURR_VAT) + g.Sum(d => d.CURR_LPS),
					   CurrMonTracedCustReceipt = g.Sum(d => d.CURR_RECEIPT_PRINCIPAL) + +g.Sum(d => d.CURR_RECEIPT_VAT),
				   });

				var previousResult = result.Item3;
				var ucDetailsPreviousDataLocWise = previousResult.GroupBy(c => c.LOCATION_CODE).Select(
					g => new UntracebleCustArrearReportDTO
					{
						ZoneCode = g.First().ZONE_CODE,
						ZoneName = g.First().ZONE_NAMEBN,
						CircleCode = g.First().CIRCLE_CODE,
						CircleNameBn = g.First().CIRCLE_NAMEBN,
						LocationCode = g.First().LOCATION_CODE,
						LOCATIONNAMEBN = g.First().LOCATION_NAMEBN,
						PreMonTracedCustCount = g.Count(),
						PreMonTracedCustArrear = g.Sum(d => d.ARREAR_PRICIPAL) + g.Sum(d => d.ARREAR_LPS) + g.Sum(d => d.ARREAR_VAT),
						PreMonTracedCustReceipt = g.Sum(d => d.ARREAR_RECEIPT_PRINCIPAL) + +g.Sum(d => d.ARREAR_RECEIPT_VAT),


					});



				var previousCurrent = ucDetailsCurrDataLocWise.Concat(ucDetailsPreviousDataLocWise);
				var combinedList = ucDetailsAllDataLocWise.Concat(previousCurrent);

				var mergeData = combinedList.GroupBy(c => c.LocationCode).Select(
				 g => new UntracebleCustArrearReportDTO
				 {
					 ZoneCode = g.First().ZoneCode,
					 ZoneName = g.First().ZoneName,
					 CircleCode = g.First().CircleCode,
					 CircleNameBn = g.First().CircleNameBn,
					 LocationCode = g.First().LocationCode,
					 LOCATIONNAMEBN = g.First().LOCATIONNAMEBN,

					 TotalUntracedCustCount = g.Sum(d => d.TotalUntracedCustCount),
					 TotalUntracedCustArrear = g.Sum(d => d.TotalUntracedCustArrear),

					 CurrMonTracedCustCount = g.Sum(c => c.CurrMonTracedCustCount),
					 CurrMonTracedCustArrear = g.Sum(c => c.CurrMonTracedCustArrear),
					 CurrMonTracedCustReceipt = g.Sum(d => d.CurrMonTracedCustReceipt),

					 PreMonTracedCustCount = g.Sum(d => d.PreMonTracedCustCount),
					 PreMonTracedCustArrear = g.Sum(d => d.PreMonTracedCustArrear),
					 PreMonTracedCustReceipt = g.Sum(d => d.PreMonTracedCustReceipt),

					 TotalCurrMonTracedCustCount = g.Sum(c => c.CurrMonTracedCustCount) + g.Sum(d => d.PreMonTracedCustCount),
					 TotalCurrMonTracedCustArrear = g.Sum(c => c.CurrMonTracedCustArrear) + g.Sum(d => d.PreMonTracedCustArrear),
					 TotalCurrMonTracedCustReceipt = g.Sum(d => d.CurrMonTracedCustReceipt) + g.Sum(d => d.PreMonTracedCustReceipt),

					 TotalFinalUntracedCustCount = g.Sum(d => d.TotalUntracedCustCount) - (g.Sum(c => c.CurrMonTracedCustCount) + g.Sum(d => d.PreMonTracedCustCount)),
					 TOTALFINALARRERARAMOUNT = g.Sum(d => d.TotalUntracedCustArrear) - (g.Sum(c => c.CurrMonTracedCustArrear) + g.Sum(d => d.PreMonTracedCustArrear))


				 });


				return mergeData.ToList();
            }
        }
    }
}
