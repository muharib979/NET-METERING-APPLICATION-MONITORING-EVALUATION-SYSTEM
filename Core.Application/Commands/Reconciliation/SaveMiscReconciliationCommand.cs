using Core.Application.Interfaces.Reconciliation;
using Shared.DTOs.Reconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Commands.Reconciliation
{
    public class SaveMiscReconciliationCommand : ReconcilationStatusDTO, IRequest<bool>
    {
        public class Handler : IRequestHandler<SaveMiscReconciliationCommand, bool>
        {
            private readonly IReconciliation _repository;

            public Handler(IReconciliation repository)
            {
                _repository = repository;
            }

            public async Task<bool> Handle(SaveMiscReconciliationCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.SaveMiscReconciliation(request);
                return result;

            }
        }
    }
}
