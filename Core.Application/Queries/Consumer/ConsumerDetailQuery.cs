using Core.Application.Interfaces.Consumer;
using Core.Application.Interfaces.Location;
using Core.Application.Queries.Location.GetLocationByUserNameAndZoneCode;
using Shared.DTOs.CustomerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Consumer
{
    public class ConsumerDetailQuery : IRequest<ConsumerDto>
    {
        public string AccountNumber { get; set; }
        public class Handler : IRequestHandler<ConsumerDetailQuery, ConsumerDto>
        {
            private readonly IConsumerRepository _repository;
            public Handler(IConsumerRepository repository)
            {
                _repository = repository;
            }

            public async Task<ConsumerDto> Handle(ConsumerDetailQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetConsumerDetails(request.AccountNumber);
                return result;
            }
        }
    }
}
