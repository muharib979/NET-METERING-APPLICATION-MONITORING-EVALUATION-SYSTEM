using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.UntracedConsumer
{
    public class ChangeConsumerStatusQuery : IRequest<bool>
    {
        public int id { get; set; }
        public int custStatus { get; set; }
        public string UpdatedBy { get; set; }

        public class Handler : IRequestHandler<ChangeConsumerStatusQuery, bool>
        {
            private readonly IUntracedConsumerRepository _repository;

            public Handler(IUntracedConsumerRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(ChangeConsumerStatusQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.ChangeConsumerStatus(request.id, request.custStatus,request.UpdatedBy);
                return result;

            }
        }
    }
}
