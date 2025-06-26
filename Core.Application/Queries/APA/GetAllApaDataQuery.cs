using Core.Application.Interfaces.APA;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA
{
    public class GetAllApaDataQuery : IRequest<List<ApaDTO>>
    {
        public class Handler : IRequestHandler<GetApaByMonthQuery, List<ApaDTO>>
        {
            private readonly IApaRepository _repository;
            public Handler(IApaRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ApaDTO>> Handle(GetApaByMonthQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetAllApaData();
                return result;

            }
        }
    }
}
