using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.PerformanceQuery
{
    public class GetAllPerformanceIndexList: IRequest<List<PerfomanceIndexDto>>
    {
        public class Handler : IRequestHandler<GetAllPerformanceIndexList, List<PerfomanceIndexDto>>
        {
            private readonly IPerformanceIndexRepository _repository;
            public Handler(IPerformanceIndexRepository programRepository)
            {
                _repository = programRepository;
            }
            public async Task<List<PerfomanceIndexDto>> Handle(GetAllPerformanceIndexList request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllPerformanceIndexList();
                return result;
            }
        }
    }
}
