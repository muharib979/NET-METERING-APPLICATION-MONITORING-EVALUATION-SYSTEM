using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA
{
    public class GetApaUnitIndexTargetQuery : IRequest<ApaUitDTO>
    {
        public string performanceCode { get; set; }
        public class Handler : IRequestHandler<GetApaUnitIndexTargetQuery, ApaUitDTO>
        {
            private readonly IApaRepository _repository;
            public Handler(IApaRepository repository)
            {
                _repository = repository;
            }

            public async Task<ApaUitDTO> Handle(GetApaUnitIndexTargetQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetApaUnitIndexTarget(request.performanceCode);
                return result;

            }
        }
    }
}
