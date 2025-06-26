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
    public class GetMisUntraceConsumerQuery:IRequest<List<UntracebleCustReportDTO>>
    {
        
        public string[] CenterCodes { get; set; }
        public string[]? LocationCodes { get; set; }
        public string BillMonth { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }


        public class Handler : IRequestHandler<GetMisUntraceConsumerQuery, List<UntracebleCustReportDTO>>
        {
            private readonly IUntraceableCustomerRepository _repository;
            private readonly IMapper _mapper;
            public Handler(IUntraceableCustomerRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper= mapper;
            }

            public async Task<List<UntracebleCustReportDTO>> Handle(GetMisUntraceConsumerQuery request, CancellationToken cancellationToken)
            {
                var result = (await _repository.GetMisUntraceableCustomer(request.CenterCodes, request.LocationCodes, request.BillMonth, request.UserId, request.RoleId));
                return result;
            }
        }
    }
}
