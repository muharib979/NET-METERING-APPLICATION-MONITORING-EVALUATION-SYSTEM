using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.UntracedConsumer
{
    public class GetConsumerSearchByDateQuery :IRequest<List<UntracedConsumerDTO>>
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? locationCode { get; set; }
        public class Handler : IRequestHandler<GetConsumerSearchByDateQuery, List<UntracedConsumerDTO>>
    {
        private readonly IUntracedConsumerRepository _repository;
        public Handler(IUntracedConsumerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UntracedConsumerDTO>> Handle(GetConsumerSearchByDateQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetConsumerSearchByDate(request.startDate, request.endDate, request.locationCode);
                return result;

        }
    }
}
}
