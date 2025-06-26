using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.LogEvent
{
    public class GetLogEventQuery : IRequest<List<LogEventDTO>>
    {
        public class Handler : IRequestHandler<GetLogEventQuery, List<LogEventDTO>>
        {
            private readonly ILogEventRepository _repository;

            public Handler(ILogEventRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<LogEventDTO>> Handle(GetLogEventQuery request, CancellationToken cancellationToken)
            {

                var result = await _repository.GetLogEvent();

                return result;

            }
        }
    }
}
