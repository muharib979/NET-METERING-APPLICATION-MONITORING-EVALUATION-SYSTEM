using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.LogEvent
{
    public class GetLogEventByDateQuery : IRequest<List<LogEventDTO>>
    {
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public class Handler : IRequestHandler<GetLogEventByDateQuery, List<LogEventDTO>>
        {
            private readonly ILogEventRepository _repository;

            public Handler(ILogEventRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LogEventDTO>> Handle(GetLogEventByDateQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetLogEventByDate(request.fromDate, request.toDate);

                return result;

            }
        }
    }
}
