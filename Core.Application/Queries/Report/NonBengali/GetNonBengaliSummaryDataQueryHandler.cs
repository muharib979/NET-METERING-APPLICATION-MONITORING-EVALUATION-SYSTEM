using Core.Application.Interfaces.NonBengali;
using Core.Domain.NonBengali;
using Shared.DTOs.Common.Wrappers;
using Shared.DTOs.NonBengali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.NonBengali
{
    public class GetNonBengaliSummaryDataQueryHandler: IRequest<Response<List<NonBengaliSummaryDTO>>>
    {
        public string ReportDate { get; set; }
        public string ReportType { get; set; }
        public class Handler : IRequestHandler<GetNonBengaliSummaryDataQueryHandler, Response<List<NonBengaliSummaryDTO>>>
        {
            private readonly INonBengaliRepository _nonBengaliRepository;
            private readonly IMapper _mapper;
            public Handler(INonBengaliRepository nonBengaliRepository, IMapper mapper)
            {
                _nonBengaliRepository = nonBengaliRepository;
                _mapper = mapper;
            }

            public async Task<Response<List<NonBengaliSummaryDTO>>> Handle(GetNonBengaliSummaryDataQueryHandler request, CancellationToken cancellationToken)
            {
                var result = await _nonBengaliRepository.GetNonBengaliConsumerSummaryByDate(request.ReportDate, request.ReportType);
                return Response<List<NonBengaliSummaryDTO>>.Success(_mapper.Map<List<NonBengaliSummaryDTO>>(result), "Success");
            }
        }
    }
}
