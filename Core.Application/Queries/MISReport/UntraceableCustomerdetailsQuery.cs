using Core.Application.Interfaces.MISReport;
using Core.Application.Queries.MISReport.UntraceableCustomerQuery;
using Core.Domain.Untracable;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MISReport
{
    public class UntraceableCustomerdetailsQuery : IRequest<Response<List<UntracebleCustArrearDetailsReportDTO>>>
    {
        public string ZoneCode { get; set; }
        public string CircleCode { get; set; }
        public string LocationCode { get; set; }
        public string BillMonth { get; set; }


        public class Handler : IRequestHandler<UntraceableCustomerdetailsQuery, Response<List<UntracebleCustArrearDetailsReportDTO>>>
        {
            private readonly IUntraceableCustomerRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUntraceableCustomerRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Response<List<UntracebleCustArrearDetailsReportDTO>>> Handle(UntraceableCustomerdetailsQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetUntraceableArrearDetails(request.ZoneCode, request.CircleCode, request.LocationCode, request.BillMonth);

                return Response<List<UntracebleCustArrearDetailsReportDTO>>.Success(_mapper.Map<List<UntracebleCustArrearDetailsReportDTO>>(result), "Success");



            }
        }
    }
}
