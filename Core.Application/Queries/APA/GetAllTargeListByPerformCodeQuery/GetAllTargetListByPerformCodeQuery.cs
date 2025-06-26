using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetAllTargeListByPerformCodeQuery
{
    public class GetAllTargetListByPerformCodeQuery : IRequest<List<TargetDto>>
    {
        public string PerformCode { get; set; }
        public class Handler : IRequestHandler<GetAllTargetListByPerformCodeQuery, List<TargetDto>>
        {
            private readonly IUnitIndexTargetRepository _targetRepository;
            public Handler(IUnitIndexTargetRepository targetRepository)
            {
                _targetRepository = targetRepository;
            }

            public async Task<List<TargetDto>> Handle(GetAllTargetListByPerformCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _targetRepository.GetAllTargetDataByPerformanceCode(request.PerformCode);
                return result;

            }
        }
    }
}
