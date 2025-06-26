using Core.Application.Interfaces.MiscBilling;
using Shared.DTOs.MISCBILL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.MiscBilling.LogEvent
{
    public class DeleteLogEventCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class Handler : IRequestHandler<DeleteLogEventCommand, int>
        {
            private readonly ILogEventRepository _repository;

            public Handler(ILogEventRepository repository)
            {
                _repository = repository;
            }

            public async Task<int> Handle(DeleteLogEventCommand request, CancellationToken cancellationToken)
            {

                var result = await _repository.DeleteLogEvent(request.Id);

                return result;

            }
        }
    }
}
