using Core.Application.Interfaces.APA;
using Core.Application.Queries.APA.GetAllTargeListByPerformCodeQuery;
using Shared.DTOs.APA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.APA
{
    public class GetApaByMonthQuery : IRequest<List<ApaDTO>>
    {
        public string billMonth { get; set; }
        public class Handler : IRequestHandler<GetApaByMonthQuery, List<ApaDTO>>
        {
            private readonly IApaRepository _repository;
            public Handler(IApaRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ApaDTO>> Handle(GetApaByMonthQuery request, CancellationToken cancellationToken)
            {
                var result = await _repository.GetApaByMonth(request.billMonth);
                return result;

            }
        }
    }
}
