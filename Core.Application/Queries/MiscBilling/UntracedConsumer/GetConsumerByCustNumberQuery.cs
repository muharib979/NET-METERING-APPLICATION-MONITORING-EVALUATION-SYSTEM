using Core.Application.Interfaces.APA;
using Core.Application.Interfaces.MiscBilling;
using Core.Application.Queries.APA;
using Shared.DTOs.APA;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.UntracedConsumer
{
    public class GetConsumerByCustNumberQuery : IRequest<UntracedConsumerDTO>
    {
        public string custNumber { get; set; }
        public class Handler : IRequestHandler<GetConsumerByCustNumberQuery, UntracedConsumerDTO>
        {
            private readonly IUntracedConsumerRepository _repository;
            public Handler(IUntracedConsumerRepository repository)
            {
                _repository = repository;
            }


            public async Task<UntracedConsumerDTO> Handle(GetConsumerByCustNumberQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetConsumerByCustNumber(request.custNumber);
                return result;

            }
        }
    }
}
