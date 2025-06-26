using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA.GetAllUnitIndexAndPerformValueByIndexCode
{
    public class GetAllUnitIndexAndPerformValueByIndexCodeQuery: IRequest<List<UnitIndexAndPerformValueDto>>
    {
        public string IndexCode { get; set; }
        public class Handler : IRequestHandler<GetAllUnitIndexAndPerformValueByIndexCodeQuery, List<UnitIndexAndPerformValueDto>>
        {
            private readonly IUnitIndexTargetRepository _unitIndexValueRepository;
            public Handler(IUnitIndexTargetRepository unitIndexValueRepository)
            {
                _unitIndexValueRepository = unitIndexValueRepository;
            }

            public async Task<List<UnitIndexAndPerformValueDto>> Handle(GetAllUnitIndexAndPerformValueByIndexCodeQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitIndexValueRepository.GetAllUnitIndexAndPerformValueByIndexCode(request.IndexCode);
                return result;
            }
        }
    }
}
