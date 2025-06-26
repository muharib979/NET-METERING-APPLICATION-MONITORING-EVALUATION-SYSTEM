using Core.Application.Interfaces.Ministry;
using Shared.DTOs.Ministry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Queries.Report.Ministry.GetAllUnionPorishod
{
    public class GetAllUnionPorishodQuery: IRequest<List<UnionPorishodDTO>>
    {
        public class Handler : IRequestHandler<GetAllUnionPorishodQuery, List<UnionPorishodDTO>>
        {
            private readonly IMinistryRepository _ministryRepository;
            private readonly IMapper _mapper;
            public Handler(IMinistryRepository ministryRepository, IMapper mapper)
            {
                _ministryRepository = ministryRepository;
                _mapper = mapper;
            }

            public async Task<List<UnionPorishodDTO>> Handle(GetAllUnionPorishodQuery request, CancellationToken cancellationToken)
            {
                var result = await _ministryRepository.GetAllUnionPorishod();
                return result;
            }
        }
    }
}
