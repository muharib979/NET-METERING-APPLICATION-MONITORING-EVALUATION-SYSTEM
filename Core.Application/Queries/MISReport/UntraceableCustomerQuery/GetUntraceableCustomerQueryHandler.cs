using Core.Application.Interfaces.MISReport;
using Core.Domain.Untracable;
using Shared.DTOs.MISReport;

namespace Core.Application.Queries.MISReport.UntraceableCustomerQuery
{
    public class GetUntraceableCustomerQueryHandler : IRequest<List<MergeUntraceableDto>>
    {
        //public string zoneCode { get; set; }
        //public string locationCode { get; set; }
        public string ReportDate { get; set; }

        public class Handler : IRequestHandler<GetUntraceableCustomerQueryHandler, List<MergeUntraceableDto>>
        {
            private readonly IUntraceableCustomerRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IUntraceableCustomerRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }
            public async Task<List<MergeUntraceableDto>> Handle(GetUntraceableCustomerQueryHandler request, CancellationToken cancellationToken)
            {
                var result = _mapper.Map<List<MergeUntraceableDto>>(await _repository.GetAllIUntraceableCustomer(request.ReportDate));
                return result;
            }
        }
    }
}
