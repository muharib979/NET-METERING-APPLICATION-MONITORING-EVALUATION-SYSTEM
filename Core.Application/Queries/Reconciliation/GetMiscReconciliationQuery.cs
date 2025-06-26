using Core.Application.Interfaces.Reconciliation;
using Shared.DTOs.Reconciliation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Reconciliation
{
    public class GetMiscReconciliationQuery : IRequest<List<ReconcilationStatusDTO>>
    {
        public string? startDate { get; set; }
        public string? endDate { get; set; }
        public string? user { get; set; }
        public class Handler : IRequestHandler<GetMiscReconciliationQuery, List<ReconcilationStatusDTO>>
        {
            private readonly IReconciliation _repository;
            public Handler(IReconciliation programRepository)
            {
                _repository = programRepository;
            }


            public async Task<List<ReconcilationStatusDTO>> Handle(GetMiscReconciliationQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetMiscReconcilation(request.startDate, request.endDate,request.user);
                return result;
            }
        }
    }
}
