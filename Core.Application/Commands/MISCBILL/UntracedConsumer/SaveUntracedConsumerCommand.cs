using Core.Application.Commands.APABILL;
using Core.Application.Interfaces.APA;
using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.APA;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.MISCBILL.UntracedConsumer
{
    public class SaveUntracedConsumerCommand : UntracedConsumerDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveUntracedConsumerCommand, bool>
        {
            private readonly IUntracedConsumerRepository _repository;

            public Handler(IUntracedConsumerRepository repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveUntracedConsumerCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveUntracedConsumer(request);
                return result;

            }
        }
    }
}
