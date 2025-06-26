using Core.Application.Interfaces.Reconciliation;
using Shared.DTOs.Reconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Reconciliation
{
    public class SaveConsumerReconciliationCommand : ReconcilationStatusDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveConsumerReconciliationCommand, bool>
        {
            private readonly IReconciliation _repository;

            public Handler(IReconciliation repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveConsumerReconciliationCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveConsumerReconciliation(request);
                return result;

            }
        }
    }
}
