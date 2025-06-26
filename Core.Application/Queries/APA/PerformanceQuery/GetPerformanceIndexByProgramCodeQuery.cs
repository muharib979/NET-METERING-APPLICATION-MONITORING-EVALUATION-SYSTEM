using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetPerformanceIndexByProgramCodeQuery
{
    public class GetPerformanceIndexByProgramCodeQuery : IRequest<List<PerfomanceIndexDto>>
    {
        public string ProgramCode { get; set; }
        public class Handler : IRequestHandler<GetPerformanceIndexByProgramCodeQuery, List<PerfomanceIndexDto>>
        {
            private readonly IPerformanceIndexRepository _performanceRepository;
            public Handler(IPerformanceIndexRepository performanceRepository)
            {
                _performanceRepository = performanceRepository;
            }
            public async Task<List<PerfomanceIndexDto>> Handle(GetPerformanceIndexByProgramCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _performanceRepository.GetAllPerformanceIndexDataByProgramCode(request.ProgramCode);
                return result;
            }
        }
    }
}
