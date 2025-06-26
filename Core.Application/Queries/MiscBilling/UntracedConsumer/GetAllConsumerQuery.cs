using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.UntracedConsumer
{
    public class GetAllConsumerQuery : IRequest<List<UntracedConsumerDTO>>
    {
        public class Handler : IRequestHandler<GetAllConsumerQuery, List<UntracedConsumerDTO>>
        {
            private readonly IUntracedConsumerRepository _repository;
            public Handler(IUntracedConsumerRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<UntracedConsumerDTO>> Handle(GetAllConsumerQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllConsumer();
                return result;

            }
        }
    }
}
